using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Vim;
using Vim.DataFormat;
using Vim.DotNetUtilities;
using Vim.Geometry;
using Vim.LinqArray;
using Vim.Math3d;

namespace BIMSocket_VIM
{
    public static class VimHackerProgram
    {
        public static string TestDataFolder => Path.Combine(Assembly.GetExecutingAssembly().Location, "..", "..", "..", "..", "test-data");
        public static string TestInputFolder => Path.Combine(TestDataFolder, "input");
        public static string TestOutputFolder => Path.Combine(TestDataFolder, "output");

        public static string JsonTestBimSocketFile = Path.Combine(TestInputFolder, "BIMSocket.json");
        public static string JsonRstSampleProject = Path.Combine(TestInputFolder, "rst_basic_sample_project.rvt.json");
        public static string JsonRacSampleProject = Path.Combine(TestInputFolder, "rac_basic_sample_project.rvt.json");

        public static string VimWolfordHouse = Path.Combine(TestInputFolder, "Wolford_Residence_2019.vim");

        public static Va3cContainer LoadVa3c(string filePath)
        {
            using (var file = File.OpenText(filePath))
                return (Va3cContainer)new JsonSerializer().Deserialize(file, typeof(Va3cContainer));
        }

        public static void Write(this Va3cContainer va3c, string filePath)
        {
            using (var file = File.CreateText(filePath))
                new JsonSerializer().Serialize(file, va3c);
        }

        public static Va3cGeometry ToGeometry(IMesh m, int index)
        {
            var r = new Va3cGeometry();
            r.uuid = index.ToString();
            for (var i = 0; i < m.NumFaces; ++i)
            {
                r.data.faces.Add(0);
                for (var j = 0; j < 3; ++j)
                    r.data.faces.Add(m.Indices[i * 3 + j]);
            }

            for (var i = 0; i < m.NumVertices; ++i)
            {
                var v = m.Vertices[i];
                r.data.vertices.Add(v.X * Constants.FeetToMm);
                r.data.vertices.Add(v.Y * Constants.FeetToMm);
                r.data.vertices.Add(v.Z * Constants.FeetToMm);
            }
            return r;
        }

        public static Va3cObject ToObject(VimSceneNode node)
            => new Va3cObject
            {
                geometry = node._Source.Geometry >= 0
                    ? node._Source.Geometry.ToString()
                    : ""
            };

        public static Va3cContainer ToVa3c(this VimScene vim)
        {
            var r = new Va3cContainer();
            r.geometries = vim.Geometries.Select(ToGeometry).ToList();
            r.obj.children = vim.VimNodes.Select(ToObject).ToList();
            return r;
        }

        public const float Mult = (float)Constants.MmToFeet;

        public static Vector3 ToVertex(double x, double y, double z)
            => new Vector3((float)x * Mult, (float)y * Mult, (float)z * Mult);

        public static GeometryBuilder ToGeometryBuilder(Va3cGeometry g)
        {
            var gb = new GeometryBuilder();
            var vertices = g.data.vertices.ToIArray()
                .SelectTriplets(ToVertex);

            // https://github.com/mrdoob/three.js/wiki/JSON-Model-format-3
            // https://github.com/mrdoob/three.js/wiki/JSON-Object-Scene-format-4
            // https://stackoverflow.com/questions/35386518/parsing-three-js-json-mesh-format-normals-errors
            // https://stackoverflow.com/questions/28023734/threejs-json-loader-mixed-faces-and-vertices

            var f = 0;
            while (f < g.data.faces.Count)
            {
                var bits = g.data.faces[f++];

                var isTriangle = (bits & 1) == 0;
                var isQuad = (bits & 1) != 0;
                var hasMaterial = (bits & 2) != 0;
                var hasUV = (bits & 4) != 0;
                var hasVertexUv = (bits & 8) != 0;
                var hasNormal = (bits & 16) != 0;
                var hasVertexNormal = (bits & 32) != 0;
                var hasColor = (bits & 64) != 0;
                var hasVertexColor = (bits & 128) != 0;

                Debug.Assert(bits == 0);

                for (var i = 0; i < 3; ++i)
                    gb.Indices.Add(g.data.faces[f++]);
            }

            gb.Vertices.AddRange(vertices.ToArray());
            gb.UVs.AddRange(Vector2.Zero.Repeat(vertices.Count).ToArray());

            var nfaces = gb.Indices.Count / 3;
            for (var i = 0; i < nfaces; ++i)
            {
                gb.MaterialIds.Add(-1);
                gb.FaceGroupIds.Add(-1);
            }
            return gb;
        }

        public static void ProcessNode(DocumentBuilder db, Va3cObject obj, Dictionary<string, int> geometryLookup)
        {
            var transform = Matrix4x4.Identity;
            var geoIndex = geometryLookup.ContainsKey(obj.geometry) ? geometryLookup[obj.geometry] : -1;
            db.AddNode(transform, geoIndex, -1, -1);
            foreach (var c in obj.children)
                ProcessNode(db, c, geometryLookup);
        }

        public static VimScene ToVim(this Va3cContainer va3c)
        {
            var db = new DocumentBuilder();
            var geometryLookup = new Dictionary<string, int>();
            foreach (var g in va3c.geometries)
                geometryLookup.Add(g.uuid, geometryLookup.Count);
            db.AddGeometries(va3c.geometries.Select(ToGeometryBuilder));
            ProcessNode(db, va3c.obj, geometryLookup);
            return new VimScene(db.ToDocument());
        }

        public static void SaveAsVim(this Va3cContainer va3c, string filePath)
            => va3c.ToVim().Save(filePath);

        public static void SaveAsVa3c(this VimScene vim, string filePath)
            => vim.ToVa3c().Write(filePath);

        public static string TestJsonToVim(string filePath, bool roundTrip)
        {
            // Try loading the JSON and saving as VIM
            var va3c = LoadVa3c(filePath);
            var outputVim = Util.ChangeDirectoryAndExt(filePath, TestOutputFolder, ".vim");
            va3c.SaveAsVim(outputVim);

            // TODO: currently fails
            if (roundTrip)
                return TestVimToJsonToVim(outputVim);

            return outputVim;
        }

        public static string TestVimToJsonToVim(string filePath)
        {
            // Check we can open the VIM
            var vim = VimScene.LoadVim(filePath);

            // Save the VIM as JSON
            var outputJson = Path.ChangeExtension(filePath, ".json");
            vim.SaveAsVa3c(outputJson);

            // Now try reloading thw new JSON and saving again as VIM
            var va3c = LoadVa3c(outputJson);
            var outputVim = Util.ChangeDirectoryAndExt(filePath, TestOutputFolder, ".resaved.vim");
            va3c.SaveAsVim(outputVim);
            return outputVim;
        }

        public static string TestVimToObj(string filePath, string objFilePath = null)
        {
            objFilePath = objFilePath ?? Util.ChangeDirectoryAndExt(filePath, TestOutputFolder, ".obj");
            VimScene.LoadVim(filePath).ToIMesh().WriteObj(objFilePath);
            return objFilePath;
        }

        public static string TestJsonToObj(string filePath, string objFilePath = null)
        {
            objFilePath = objFilePath ?? Util.ChangeDirectoryAndExt(filePath, TestOutputFolder, ".obj");
            LoadVa3c(filePath).ToVim().ToIMesh();
            return objFilePath;
        }

        public static void Main(string[] args)
        {
            //var output = TestJsonToVim(RacSampleProject);

            // TEMP: broken
            //var output = TestJsonToObj(JsonRacSampleProject);

            var output = TestVimToObj(VimWolfordHouse);

            Process.Start(output);
        }
    }
}
