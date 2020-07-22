using System;
using System.Dynamic;
using Newtonsoft.Json;
using System.Drawing;

namespace RCva3c
{
    public class va3c_LineBasicMaterial
    {
        public Material GenerateMaterial( Color inColor, double inNumber = 1.0)
        {
            //spin up a JSON material
            string outJSON = ConstructMaterial(inColor, inNumber);

            Material material = new Material(outJSON, va3cMaterialType.Line);
            return material;
        }

        private string ConstructMaterial(Color inColor, double inNumber)
        {
            //json object to populate
            dynamic jason = new ExpandoObject();

            //JSON properties
            jason.uuid = Guid.NewGuid();
            jason.type = "LineBasicMaterial";
            jason.color = _Utilities.hexColor(inColor);
            jason.linewidth = inNumber;
            jason.opacity = 1;


            return JsonConvert.SerializeObject(jason);
        }
    }
}