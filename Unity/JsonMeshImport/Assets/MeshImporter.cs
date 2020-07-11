using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Assets;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine.UIElements;

[RequireComponent(typeof(MeshFilter))]
public class MeshImporter : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        LoadJson();
        CreateShape(importModel);
        
        
    }

    void CreateShape(Rootobject importedJson)
    {

        Geometry[] geom = importedJson.geometries;
        foreach (Geometry g in geom)
        {
            if(g.data.faces.Length % 3 == 0)
            { 
            //float?[] RawVertices = g.data.vertices;
            vertices = ConvertToVector3Array(g.data.vertices);
            triangles = g.data.faces;
            UpdateMesh(mesh, vertices, triangles);
            }
        }

        //vertices = new Vector3[]
        //{
        //    new Vector3 (0,0,0),
        //    new Vector3 (0,0,1),
        //    new Vector3 (1,0,0)
        //};

        //int?[] faces

        //triangles = new int[]
        //{
        //    0, 1, 2
        //};

    }

    Vector3[] ConvertToVector3Array(float[] floats)
    {
        List<Vector3> vector3List = new List<Vector3>();
        int scale = 1000;
        for (int i = 0; i < floats.Length; i += 3)
        {
            vector3List.Add(new Vector3(floats[i]/scale, floats[i + 1]/scale, floats[i + 2]/scale));
        }
        return vector3List.ToArray();
    }

    Vector3[] generateVector(float?[] vertices)
    {
        Vector3[] result = new Vector3[] { };
        IEnumerable <List<float?>> verticesArray = SplitArrayFloat(vertices, 3);   
        int count = 0;
        foreach (List<float?> i in verticesArray)
       {
            Vector3 tempVector = new Vector3();
            tempVector.x = (float)i[0];
            tempVector.y = (float)i[1];
            tempVector.z = (float)i[2];
            result[count] = tempVector;
            count++;
       }
        return result;
    }

    int[] generateTriangles(int?[] faces)
    {
        int[] result = faces.Cast<int>().ToArray();
        return result;
    }


    void UpdateMesh(Mesh mesh, Vector3[] vertices, int[] triangles)
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private Rootobject importModel;
    void LoadJson()
    {
        using(StreamReader stream = new StreamReader("C:\\Users\\vnoves\\Desktop\\Roof.js"))
        {
            string jsonString = stream.ReadToEnd();
            Debug.Log(jsonString);
            importModel = JsonConvert.DeserializeObject<Rootobject>(jsonString);
        }
    }

    /// <summary>
    /// Splits an array into several smaller arrays.
    /// </summary>
    /// <typeparam name="T">The type of the array.</typeparam>
    /// <param name="array">The array to split.</param>
    /// <param name="size">The size of the smaller arrays.</param>
    /// <returns>An array containing smaller arrays.</returns>
    public static IEnumerable<List<float?>> SplitArrayFloat(float?[] floatArray, int size)
    {
        for (var i = 0; i < (float)floatArray.Length / size; i++)
        {
            yield return (List<float?>)floatArray.Skip(i * size).Take(size);
        }
    }
}
