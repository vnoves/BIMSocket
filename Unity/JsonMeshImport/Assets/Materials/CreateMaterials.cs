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
            UnityEngine.Color color = GetColour(mat);
            // Create a simple material asset
            var material = new UnityEngine.Material(Shader.Find("Standard"));
            material.SetColor("_Color", color);
            string path = "Assets/Resources/" + strRes + ".mat";
            var myType = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Material)) as UnityEngine.Material;
            if (myType == null) 
            {
                AssetDatabase.CreateAsset(material, path);
            }
            else {
                myType.SetColor("_Color", color);
            }
        }

        private static UnityEngine.Color GetColour(Material mat)
        {
            Vector3 colorRGB = IntToRgb(mat.color);
            var opacityInt = mat.opacity;
                UnityEngine.Color colorUnity =
                    new UnityEngine.Color(colorRGB.x / 255, colorRGB.y / 255, colorRGB.z / 255, opacityInt);
                return colorUnity;       
        }

        public static Vector3 IntToRgb(int value)
        {
            var red = (value >> 16) & 255;
            var green = (value >> 8) & 255;
            var blue = (value >> 0) & 255;
            return new Vector3(red, green, blue);
        }
    }
}
