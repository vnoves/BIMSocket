using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Geometries
{
    public static class Geometries
    {
        public static List<GameObject> create(List<Geometry> geometries)
        {
            List<GameObject> gObjCreated = new List<GameObject>();
            foreach (Geometry g in geometries)
            {
                if (g.data.faces.Length > 0)
                {
                    Vector3[] vertices = ConvertToVector3Array(g.data.vertices);
                    int[] triangles = removeFourthValue(g.data.faces);
                    GameObject newObject = new GameObject(g.uuid.ToString());
                    UnityEngine.Mesh tempMesh = createGameObject(newObject);
                    UpdateMesh(tempMesh, vertices, triangles);
                    gObjCreated.Add(newObject);
                }
            }
            return gObjCreated;
        }

        private static Vector3[] ConvertToVector3Array(float[] floats)
        {
            List<Vector3> vector3List = new List<Vector3>();
            int scale = 1000;
            for (int i = 0; i < floats.Length; i += 3)
            {
                vector3List.Add(new Vector3(floats[i] / scale, floats[i + 1] / scale, floats[i + 2] / scale));
            }
            return vector3List.ToArray();
        }

        private static int[] removeFourthValue(int[] floats)
        {
            int[] FacesList = new int[] { };
            int Count = 0;
            for (int i = 0; i <= floats.Length; i++)
            {
                if (Count == 0 || Count == 4)
                {
                    Count = 0;
                }
                else
                {
                    FacesList = (FacesList ?? Enumerable.Empty<int>()).Concat(new[] { floats[i] }).ToArray();
                }
                Count++;
            }
            return FacesList;
        }

        private static UnityEngine.Mesh createGameObject(GameObject newObject)
        {
            
            UnityEngine.Mesh mesh = new UnityEngine.Mesh();
            MeshFilter meshFilter = newObject.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = mesh;
            MeshRenderer meshRenderer = newObject.AddComponent<MeshRenderer>();      
            return mesh;
        }

        private static void UpdateMesh(UnityEngine.Mesh mesh, Vector3[] vertices, int[] triangles)
        {
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            
        }
    }

}
