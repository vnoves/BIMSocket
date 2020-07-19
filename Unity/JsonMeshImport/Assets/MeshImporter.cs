using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Assets;
using Assets.Geometries;
using Assets.Json;
using Assets.Materials;
using Firebase.Firestore;
using Firebase;
using UnityEngine.SceneManagement;

//using BIMSocket;


[RequireComponent(typeof(MeshFilter))]
public class MeshImporter : MonoBehaviour
{
    public static FirebaseFirestore DB;
    UnityEngine.Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    private Rootobject importModel;
    // Start is called before the first frame update
    void Start()
    {
        //Connect to Firebase
        DB = FirebaseFirestore.GetInstance(FirebaseApp.DefaultInstance);
        
        FBModelImport();
        string pathLocation = "C:\\Users\\vnoves\\Desktop\\RoofAndWalls.js";
        //Import Json
        //Import.LoadJson(ref importModel, pathLocation);
        //Create all the Materials
         
    }

    public async void FBModelImport()
    {
        Rootobject result = new Rootobject();

        //var allCitiesQuery = DB.Collection("models");
        //var docModel = allCitiesQuery.Document("test1");
        //Firebase.Firestore.DocumentSnapshot snapshot = await docModel.GetSnapshotAsync();


        //if (snapshot.Exists)
        //   {
        //    var st = Newtonsoft.Json.JsonConvert.SerializeObject(snapshot.ToDictionary());
        //    importModel = Newtonsoft.Json.JsonConvert.DeserializeObject<Rootobject>(st);

        //    CreateMaterials.create(importModel.materials);
        //    //Create Main Object
        //    GameObject MainObj = new GameObject(importModel._object.name);
        //    //Create geometries
        //    List<GameObject> createdGeom = Geometries.create(importModel.geometries);
        //    //Create Childrens and assign them to Main Object
        //    ObjChildrens.create(importModel._object.children, MainObj, createdGeom);
        //}
        //    else
        //    {
        //    Debug.Log("Document {0} does not exist!" + snapshot.Id.ToString());
        //    }

        try
        {
            DocumentReference docRef = DB.Collection("models").Document("test1");    
            var listener = docRef.Listen(snapshot =>
            {
                if (snapshot.Exists)
                {
                    var st = Newtonsoft.Json.JsonConvert.SerializeObject(snapshot.ToDictionary());
                    importModel = Newtonsoft.Json.JsonConvert.DeserializeObject<Rootobject>(st);
                    //If the element already exists remove it
                    var gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
                    foreach(GameObject go in gameObjects)
                    {
                        if(go.name == importModel._object.name)
                        {
                            Destroy(go);
                        }
                    }
                    CreateMaterials.create(importModel.materials);
                    //Create Main Object
                    //Create main Game object
                    GameObject MainObj = new GameObject(importModel._object.name);           
                    //Create geometries
                    List<GameObject> createdGeom = Geometries.create(importModel.geometries);
                    //Create Childrens and assign them to Main Object
                    ObjChildrens.create(importModel._object.children, MainObj, createdGeom);

                }
            });
        }
        catch
        {
            
        }

    }

  


}
