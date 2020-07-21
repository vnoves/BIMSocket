using System;
using System.Dynamic;
using Newtonsoft.Json;
using System.Drawing;

namespace RCva3c
{
    public class va3c_MeshPhongMaterial
    {
        /// <summary>
        /// Generates Mesh Phong Material. Feed this into the vA3C Mesh component.
        /// </summary>
        /// <param name="inColor">Diffuse color of the material</param>
        /// <param name="inAmbient">Ambient color of the material, multiplied by the color of the ambient light in the scene.  Default is black</param>
        /// <param name="inEmissive">Emissive (light) color of the material, essentially a solid color unaffected by other lighting. Default is black.</param>
        /// <param name="inSpecular">Specular color of the material, i.e., how shiny the material is and the color of its shine. Setting this the same color as the diffuse value (times some intensity) makes the material more metallic-looking; setting this to some gray makes the material look more plastic. Default is dark gray.</param>
        /// <param name="inShininess">How shiny the specular highlight is; a higher value gives a sharper highlight. Default is 30</param>
        /// <param name="inOpacity">Number in the range of 0.0 - 1.0 indicating how transparent the material is. A value of 0.0 indicates fully transparent, 1.0 is fully opaque.</param>
        /// <returns></returns>
        public Material GeneratePhongMaterial(Color inColor, Color inAmbient, Color inEmissive, Color inSpecular, double inShininess, double inOpacity)
        {
            if (inAmbient == null)
            {
                inAmbient = Color.Black;
            }
            if (inEmissive == null)
            {
                inEmissive = Color.Black;
            }
            if (inSpecular == null)
            {
                inSpecular = Color.DarkGray;
            }

            string outMaterial = null;

            if (inOpacity > 1 || inOpacity < 0)
            {
                inOpacity = 1.0;
            }

            outMaterial = ConstructPhongMaterial(inColor, inAmbient, inEmissive, inSpecular, inShininess, inOpacity);
            //call json conversion function

            Material material = new Material(outMaterial, va3cMaterialType.Mesh);
            return material;
        }

        private string ConstructPhongMaterial(Color col, Color amb, Color em, Color spec, double shin, double opp)
        {
            dynamic jason = new ExpandoObject();

            jason.uuid = Guid.NewGuid();
            jason.type = "MeshPhongMaterial";
            jason.color = _Utilities.hexColor(col);
            jason.ambient = _Utilities.hexColor(amb);
            jason.emissive = _Utilities.hexColor(em);
            jason.specular = _Utilities.hexColor(spec);
            jason.shininess = shin;
            if (opp < 1)
            {
                jason.transparent = true;
                jason.opacity = opp;
            }
            jason.wireframe = false;
            jason.side = 2;
            return JsonConvert.SerializeObject(jason);
        }
    }
}