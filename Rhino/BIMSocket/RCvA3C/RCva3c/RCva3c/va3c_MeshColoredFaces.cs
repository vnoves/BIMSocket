using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Linq;
using Rhino.Geometry;
using Newtonsoft.Json;
using System.Drawing;

namespace RCva3c
{
    public class va3c_MeshColoredFaces
    {
        /// <summary>
        /// Generates mesh colored faces element
        /// </summary>
        /// <param name="mesh">Rhinocommon mesh</param>
        /// <param name="colors">A list of colors - one per face.</param>
        /// <param name="attributeNames">Attribute Names</param>
        /// <param name="attributeValues">Attribute Values</param>
        /// <param name="layerName">Layer</param>
        /// <returns></returns>
        public Element GenerateMeshColoredFacesEelement(Mesh mesh, Guid uuid, List<Color> colors, List<string> attributeNames, List<string> attributeValues, string layerName = "Default")
        {
            //local varaibles
            attributeNames = new List<string>();
            attributeValues = new List<string>();
            Dictionary<string, object> attributesDict = new Dictionary<string, object>();
            Layer layer = null;

            //catch inputs and populate local variables

            if (mesh == null)
            {
                return null;
            }

            if (attributeValues.Count != attributeNames.Count)
            {
                return null;
            }

            layer = new Layer(layerName);

            //populate dictionary
            int i = 0;
            foreach (var a in attributeNames)
            {
                attributesDict.Add(a, attributeValues[i]);
                i++;
            }

            //add the layer name to the attributes dictionary
            attributesDict.Add("layer", layerName);

            //create MeshFaceMaterial and assign mesh face material indexes in the attributes dict
            string meshMaterailJSON = makeMeshFaceMaterialJSON(mesh, attributesDict, colors);

            //create json from mesh
            string meshJSON = _Utilities.geoJSON(mesh, uuid, attributesDict);

            Material material = new Material(meshMaterailJSON, va3cMaterialType.Mesh);
            Element e = new Element(meshJSON, va3cElementType.Mesh, material, layer);

            return e;

        }

        private string makeMeshFaceMaterialJSON(Mesh mesh, Dictionary<string, object> attributesDict, List<Color> colors)
        {
            //JSON object to populate
            dynamic jason = new ExpandoObject();
            jason.uuid = Guid.NewGuid();
            jason.type = "MeshFaceMaterial";

            //we need an list of material indexes, one for each face of the mesh.  This will be stroed as a CSV string in the attributes dict
            //and on the viewer side we'll use this to set each mesh face's material index property
            List<int> myMaterialIndexes = new List<int>();

            //since some faces might share a material, we'll keep a local dict of materials to avoid duplicates
            //key = hex color, value = int representing a material index
            Dictionary<string, int> faceMaterials = new Dictionary<string, int>();

            //we'll loop over the mesh to make sure that each quad is assigned two material indexes
            //since it is really two triangles as a three.js mesh.  If there are fewer colors than mesh faces, we'll take the last material
            int matCounter = 0;
            int uniqueColorCounter = 0;
            foreach (var f in mesh.Faces)
            {
                //make sure there is an item at this index.  if not, grab the last one
                if (matCounter == mesh.Faces.Count)
                {
                    matCounter = mesh.Faces.Count = 1;
                }

                //get a string representation of the color
                string myColorStr = _Utilities.hexColor(colors[matCounter]);

                //check to see if we need to create a new material index
                if (!faceMaterials.ContainsKey(myColorStr))
                {
                    //add the color/index pair to our dictionary and increment the unique color counter
                    faceMaterials.Add(myColorStr, uniqueColorCounter);
                    uniqueColorCounter++;
                }

                //add the color[s] to the array.  one for a tri, two for a quad
                if (f.IsTriangle)
                {
                    myMaterialIndexes.Add(faceMaterials[myColorStr]);
                }
                if (f.IsQuad)
                {
                    myMaterialIndexes.Add(faceMaterials[myColorStr]);
                    myMaterialIndexes.Add(faceMaterials[myColorStr]);
                }
                matCounter++;
            }

            //now that we know how many unique materials we need, we'll create a materials array on jason, and add them all to it
            jason.materials = new object[faceMaterials.Count];
            for (int i = 0; i < faceMaterials.Count; i++)
            {
                dynamic matthew = new ExpandoObject();
                matthew.uuid = Guid.NewGuid();
                matthew.type = "MeshBasicMaterial";
                matthew.side = 2;
                matthew.color = faceMaterials.Keys.ToList()[i];
                jason.materials[i] = matthew;
            }

            //finally, we need to add a csv string of the materials to our attribute dictionary
            attributesDict.Add("VA3C_FaceColorIndexes", createCsvString(myMaterialIndexes));
            return JsonConvert.SerializeObject(jason);
        }

        //method to create a csv string out of a list of integers
        private object createCsvString(List<int> myMaterialIndexes)
        {
            string csv = "";
            foreach (var i in myMaterialIndexes)
            {
                csv = csv + i.ToString() + ",";
            }
            return csv;
        }
    }
}