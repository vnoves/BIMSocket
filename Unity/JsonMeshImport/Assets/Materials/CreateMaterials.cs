using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using System.Drawing;

namespace Assets.Materials
{
    public static class CreateMaterials
    {
        public static void create(List<Material> materials)
        {
            foreach(Material mat in materials)
            {
               
                CreateSingleMat(mat);
            }
        }
        private static void CreateSingleMat(Material mat)
        {
            string strRes = mat.uuid.Replace("/", "");
            UnityEngine.Color color = GetColour(mat.color);
            // Create a simple material asset
            var material = new UnityEngine.Material(Shader.Find("Standard"));
            material.SetColor("_Color", color);
            string path = "Assets/Resources/" + strRes + ".mat";
            var myType = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Material)) as UnityEngine.Material;
            if (myType == null) 
            {
                AssetDatabase.CreateAsset(material, path);
            }
            else { }
        }

        private static UnityEngine.Color GetColour(int intRGB)
        {
            System.Drawing.Color color =
                System.Drawing.Color.FromArgb((intRGB >> 16) & 0xff, 
                (intRGB >> 8) & 0xff, (intRGB >> 0) & 0xff);


            UnityEngine.Color colorUnity =
                new UnityEngine.Color(color.R, color.G, color.B);

            return colorUnity;
        }
    }
}
