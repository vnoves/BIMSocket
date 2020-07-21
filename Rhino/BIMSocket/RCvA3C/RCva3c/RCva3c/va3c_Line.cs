using System;
using System.Dynamic;
using Rhino.Geometry;

using Newtonsoft.Json;

namespace RCva3c
{
    public class va3c_Line
    {
        public Element GenerateLineElement(Line line, Material material, Layer layer, string layerName = "Default")
        {
            Element result = null;

            //catch inputs and populate local variables
            if (line == null || material == null)
            {
                return result;
            }

            if (material.Type != va3cMaterialType.Line)
            {
                throw new Exception("Please use a LINE Material");
            }


            //create JSON from line
            string outJSON = lineJSON(line);


            result = new Element(outJSON, va3cElementType.Line, material, layer);
            return result;
        }

        private string lineJSON(Line line)
        {
            //create a dynamic object to populate
            dynamic jason = new ExpandoObject();

            //top level properties
            jason.uuid = Guid.NewGuid();
            jason.type = "Geometry";
            jason.data = new ExpandoObject();

            //populate data object properties
            jason.data.vertices = new object[6];
            jason.data.vertices[0] = Math.Round(line.FromX * -1.0, 5);
            jason.data.vertices[1] = Math.Round(line.FromZ, 5);
            jason.data.vertices[2] = Math.Round(line.FromY, 5);
            jason.data.vertices[3] = Math.Round(line.ToX * -1.0, 5);
            jason.data.vertices[4] = Math.Round(line.ToZ, 5);
            jason.data.vertices[5] = Math.Round(line.ToY, 5);
            jason.data.normals = new object[0];
            jason.data.uvs = new object[0];
            jason.data.faces = new object[0];
            jason.data.scale = 1;
            jason.data.visible = true;
            jason.data.castShadow = true;
            jason.data.receiveShadow = false;

            //return
            return JsonConvert.SerializeObject(jason);
        }
    }
}