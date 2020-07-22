using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace RCva3c
{
    public class va3c_Mesh
    {
        /// <summary>
        /// Generates mesh element that can be used with scene compiler
        /// </summary>        
        /// <param name="mesh">A Rhinocommon Mesh</param>
        /// <param name="material">Mesh Material</param>
        /// <param name="attributeNames">Attribute Names</param>
        /// <param name="attributeValues">Attribute Values</param>
        /// <param name="layerName">Layer</param>
        /// <returns></returns>
        public Element GenerateMeshElement( Mesh mesh, Material material, Guid uuid, List<string> attributeNames, List<string> attributeValues, string layerName = "Default")
        {

            Dictionary<string, object> attributesDict = new Dictionary<string, object>();

            if (material.Type != va3cMaterialType.Mesh)
            {
                throw new Exception("Please use a MESH Material");
            }

            var layer = new Layer(layerName);


            //populate dictionary
            int i = 0;
            foreach (var a in attributeNames)
            {
                attributesDict.Add(a, attributeValues[i]);
                i++;
            }
            
            //add the layer name to the attributes dictionary
            attributesDict.Add("layer", layerName);

            //create json from mesh
            string outJSON = _Utilities.geoJSON(mesh, uuid, attributesDict);

            Element e = new Element(outJSON, va3cElementType.Mesh, material, layer);
            return e;
        }
    }
}