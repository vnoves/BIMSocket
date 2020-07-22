
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Google.Apis.Auth;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Documents;
using System.Collections.Generic;
using DocumentReference = Google.Cloud.Firestore.DocumentReference;
using Google.Apis.Util;
using System.Drawing.Drawing2D;
using BIMSocket.Utils;
using System.Dynamic;
using BIMSocket.Models;

namespace BIMSocket
{
    internal class FireBaseConnection
    {

        static string  CollectionName { get; set; }
        static string DocumentName { get; set; }

        private static FirestoreDb firestoreDb;

        /// <summary>
        /// Connect to Firestore database
        /// </summary>
        /// <param name="Collection"></param>
        /// <param name="Document"></param>
        /// <returns></returns>
        internal static bool Connect(string Collection, string Document)
        {
            CollectionName = Collection;
            DocumentName = Document;
            try
            {
                string path = FileManager.getCredentialsPath();
                FirestoreClientBuilder builder = new FirestoreClientBuilder();
                builder.CredentialsPath = path;
                var client = builder.Build();
                firestoreDb = FirestoreDb.Create("bimsocket-db", client);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// Send a first model to Firestore
        /// </summary>
        /// <param name="model"></param>
        /// <param name="CollectionName"></param>
        /// <param name="ModelName"></param>
        internal static async void SendModelToDB(string model, string CollectionName, string ModelName)
        {

            var models = firestoreDb.Collection(CollectionName).Document(ModelName);
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Rootobject>(model);
            RhinoManagement.SaveCurrentModel(obj);
            await models.SetAsync(obj);

        }

        /// <summary>
        /// Send changes to DB
        /// </summary>
        /// <param name="LocalModifications"></param>
        /// <param name="deleted"></param>
        internal static async void SendChangesToDB(string LocalModificationsString, string deleted)
        {
            try
            {
                var LocalModifications = Newtonsoft.Json.JsonConvert.DeserializeObject<Rootobject>(LocalModificationsString);
                var ServerModel = await GetModelFromServer(CollectionName, DocumentName);
                var ServerChildren = ServerModel._object.children;
                var ServerGeometry = ServerModel.geometries;
                var ServerMaterials = ServerModel.materials;
                var objectsOnly = LocalModifications._object.children.Where(x => !x.name.Contains("Camera")).ToList();
                UpdateRootObject(ServerChildren, LocalModifications._object.children);

                //Update geometry
                UpdateRootObject(ServerGeometry, LocalModifications.geometries);

                UpdateRootObject(ServerMaterials, LocalModifications.materials);
                FillEmptyChildren(ServerModel);

                RhinoManagement.SaveCurrentModel(ServerModel);
                var change = new Dictionary<FieldPath, object>();
                change[new FieldPath(new string[] { "object", "children" })] = ServerModel._object.children;
                change[new FieldPath(new string[] { "geometries" })] = ServerModel.geometries;
                await firestoreDb.Collection(CollectionName).Document(DocumentName).UpdateAsync(change);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
                return;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="CollectionName"></param>
        /// <param name="DocumentName"></param>
        /// <returns></returns>
        private static async Task<Rootobject> GetModelFromServer(string CollectionName, string DocumentName)
        {
            Rootobject RootObject = new Rootobject();
            var models = firestoreDb.Collection(CollectionName).Document(DocumentName);
            var snapshot = await models.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                Console.WriteLine("Document data for {0} document:", snapshot.Id);
                var st = Newtonsoft.Json.JsonConvert.SerializeObject(snapshot.ToDictionary());
                RootObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Rootobject>(st);

            }
            else
            {
                Console.WriteLine("Document {0} does not exist!", snapshot.Id);
            }

            return RootObject;
        }

        /// <summary>
        /// Create a listener to receive changes from Firebase
        /// </summary>
        public static void ReceiveChangesFromDB()
        {
            Rootobject RootObject = new Rootobject();
            try
            {
                DocumentReference docRef = firestoreDb.Collection(CollectionName).Document(DocumentName);
                FirestoreChangeListener listener = docRef.Listen(snapshot =>
                {
                    Console.WriteLine("Callback received document snapshot.");

                    if (snapshot.Exists)
                    {
                        Console.WriteLine("Document data for {0} document:", snapshot.Id);

                        var st = Newtonsoft.Json.JsonConvert.SerializeObject(snapshot.ToDictionary());
                        RootObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Rootobject>(st);
                        RhinoManagement.ProcessRemoteChanges(RootObject);

                        //TODO check this to detect changes
                    }
                });


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
                return;
            }
        }


        private static void FillEmptyChildren(Rootobject serverModel)
        {
            foreach (var item in serverModel._object.children)
            {
                if (item.children == null)
                {
                    item.children = new Children[] { };
                }
            }

            foreach (Geometry item in serverModel.geometries)
            {
                if (item.data.uvs == null)
                {
                    item.data.uvs = new object[0];
                }
                item.data.visible = true;
                item.data.scale = 1;
                item.data.castShadow = true;
                item.data.doubleSided = true;
                item.data.receiveShadow = true;

            }
        }

        private static void UpdateRootObject<T>(List<T> RootChildren, List<T> obj)
        {
            var objectChildrens = new List<T>();
            objectChildrens.AddRange(RootChildren);
            for (int i = 0; i < objectChildrens.Count; i++)
            {
                var item = objectChildrens[i];
                var existingObjectToUpdate = obj.Where(x => CompareUid(x, item)).FirstOrDefault();
                if (existingObjectToUpdate != null)
                {
                    RootChildren[i] = existingObjectToUpdate;
                    obj.Remove(existingObjectToUpdate);
                }

            }
            if (obj.Any())
            {
                foreach (var item in obj)
                {
                    RootChildren.Add(item);
                }
            }
        }

        private static bool CompareUid(object x, object item)
        {
            if (x is Geometry)
                return ((Geometry)x).uuid == ((Geometry)item).uuid;
            if (x is Child)
                return ((Child)x).uuid == ((Child)item).uuid;
            if (x is Material)
                return ((Material)x).uuid == ((Material)item).uuid;
            else
                return false;
        }
    }
}