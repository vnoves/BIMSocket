using System;
using System.Dynamic;
using Newtonsoft.Json;
using System.Drawing;

namespace RCva3c
{
    public class va3c_MeshLambertMaterial
    {
        /// <summary>
        /// generates mesh lambert material. Feed this into the va3c Mesh component
        /// </summary>
        /// <param name="inColor">Diffuse color of the material</param>
        /// <param name="inAmbient">Ambient color of the material, multiplied by the color of the ambient light in the scene.  Default is black</param>
        /// <param name="inEmissive">Emissive (light) color of the material, essentially a solid color unaffected by other lighting. Default is black</param>
        /// <param name="inOpacity">Number in the range of 0.0 - 1.0 indicating how transparent the material is. A value of 0.0 indicates fully transparent, 1.0 is fully opaque.</param>
        /// <param name="inSmooth">Smooth edges between faces?  If false, mesh will appear faceted.</param>
        /// <returns></returns>
        public Material GenerateMeshLambertMaterial(Color inColor, Color inAmbient, Color inEmissive, double inOpacity, bool inSmooth)
        {
            if(inAmbient == null)
            {
                inAmbient = Color.Black;
            }
            if (inEmissive == null)
            {
                inEmissive = Color.Black;
            }
            string outMaterial = null;

            if (inOpacity > 1 || inOpacity < 0)
            {
                inOpacity = 1.0;
            }

            outMaterial = ConstructLambertMaterial(inColor, inAmbient, inEmissive, inOpacity, inSmooth);
            
            Material material = new Material(outMaterial, va3cMaterialType.Mesh);

            return material;
        }

        private string ConstructLambertMaterial(Color col, Color amb, Color em, double opp, bool smooth)
        {
            dynamic jason = new ExpandoObject();

            jason.uuid = Guid.NewGuid();
            jason.type = "MeshLambertMaterial";
            jason.color = _Utilities.hexColor(col);
            jason.ambient = _Utilities.hexColor(amb);
            jason.emissive = _Utilities.hexColor(em);
            if (opp < 1)
            {
                jason.transparent = true;
                jason.opacity = opp;
            }
            jason.wireframe = false;
            jason.side = 2;
            if (smooth)
            {
                jason.shading = 2; 
            }
            else
            {
                jason.shading = 1;
            }
            return JsonConvert.SerializeObject(jason);
        }
    }
}