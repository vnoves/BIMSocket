using System;
using System.Dynamic;
using Newtonsoft.Json;
using System.Drawing;

namespace RCva3c
{
    public class va3c_MeshBasicMaterial
    {
        /// <summary>
        /// Generates material
        /// </summary>
        /// <param name="inColor"></param>
        /// <param name="inOpacity"></param>
        /// <returns></returns>
        public Material GenerateMaterial(Color inColor, double inOpacity = 1)
        {
            string outMaterial = null;
            if (inColor == null) { return null; }

            if (inOpacity > 1 || inOpacity < 0)
            {
                inOpacity = 1.0;
            }

            outMaterial = CreateMaterial(inColor, inOpacity);
            Material material = new Material(outMaterial, va3cMaterialType.Mesh);

            return material;
        }

        private string CreateMaterial(Color inColor, double inOpacity)
        {
            dynamic jason = new ExpandoObject();
            jason.uuid = Guid.NewGuid();
            jason.type = "MeshBasicMaterial";
            jason.color = _Utilities.hexColor(inColor);
            jason.side = 2;
            if (inOpacity < 1)
            {
                jason.transparent = true;
                jason.opacity = inOpacity;
            }
            return JsonConvert.SerializeObject(jason);
        }
    }
}