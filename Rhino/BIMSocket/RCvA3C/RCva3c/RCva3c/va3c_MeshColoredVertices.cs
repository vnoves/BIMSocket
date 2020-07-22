using System;
using System.Dynamic;
using System.Collections.Generic;
using Rhino.Geometry;
using Newtonsoft.Json;

namespace RCva3c
{
    public class va3c_MeshColoredVertices
    {
        /// <summary>
        /// Outputs Mesh JSON and Mesh Material JSON to feed into scene compiler component.  Make sure to amtch this material with the corresponding mesh from Mj above
        /// </summary>
        /// <param name="mesh">Rhinocommon mesh</param>
        /// <param name="attributeNames"></param>
        /// <param name="attributeValues"></param>
        /// <param name="geoJson"></param>
        /// <param name="materialWithVertexColors"></param>
        public void GenerateJsonAndMeshColoredFaces(Mesh mesh, Guid uuid, List<string> attributeNames, List<string> attributeValues, out string geoJson, out string materialWithVertexColors)
        {
            //local varaibles
            attributeNames = new List<string>();
            attributeValues = new List<string>();
            Dictionary<string, object> attributesDict = new Dictionary<string, object>();

            //populate dictionary
            int i = 0;
            foreach (var a in attributeNames)
            {
                attributesDict.Add(a, attributeValues[i]);
                i++;
            }

            //create json from mesh
            string outJSON = _Utilities.geoJSON(mesh, uuid, attributesDict);
            geoJson = outJSON;
            materialWithVertexColors = MaterialWithVertexColors();
        }

        public string MaterialWithVertexColors()
        {
            dynamic JsonMat = new ExpandoObject();

            JsonMat.uuid = Guid.NewGuid();
            JsonMat.type = "MeshLambertMaterial";
            JsonMat.color = _Utilities.hexColor(System.Drawing.Color.White);
            JsonMat.side = 2;
            JsonMat.vertexColors = 1;
            return JsonConvert.SerializeObject(JsonMat);
        }
    }
}