
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


namespace BIMSocket_VIM
{
    internal class FireBaseConnection
    {

        static string  CollectionName { get; set; }
        static string DocumentName { get; set; }

        private static FirestoreDb firestoreDb;

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
        internal static async void SendModelToDB(Rootobject model, string CollectionName, string ModelName)
        {
            Rootobject RootObject = new Rootobject();
            var models = firestoreDb.Collection(CollectionName).Document(ModelName);
            await models.SetAsync(model);

        }



            internal static async void SendChangesToDB(Rootobject LocalModifications, string deleted)
        {
            try
            {

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
                
                var change =new Dictionary<FieldPath, object>();
                change[new FieldPath(new string[] { "object","children" })] = ServerModel._object.children;
                change[new FieldPath(new string[] { "geometries" })] = ServerModel.geometries;
                await firestoreDb.Collection(CollectionName).Document(DocumentName).UpdateAsync(change);

                
                
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
                    item.children = new Child[] { };
                }
            }

            foreach (Geometry item in serverModel.geometries)
            {
                if (item.data.uvs == null)
                {
                    item.data.uvs = new List<object>();
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
                        RootObject = snapshot.ConvertTo<Rootobject>();
                       
                        VIMManagement.ConvertJsonToVim(RootObject);
                        
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

    }
}