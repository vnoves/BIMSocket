
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

namespace BIMSocket
{
    internal class FireBaseConnection
    {

        static string  CollectionName = "models";
        static string  DocumentName = "test1";

        private static FirestoreDb firestoreDb;

        internal static bool Connect()
        {
            try
            {
                string path = @"C:\Users\pderendinger\source\repos\BIMSocket\Revit\bimsocket-db-firebase-adminsdk-dy6gn-a397d4402d.json";
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


        internal static async void SendChangesToDB(Rootobject LocalModifications, string deleted)
        {
            try
            {


                var ServerModel = await GetModelFromServer(CollectionName, DocumentName);

                var ServerChildren = ServerModel._object.children;
                var ServerGeometry = ServerModel.geometries;
                var ServerMaterials = ServerModel.materials;

                var objectsOnly = LocalModifications._object.children.Where(x => !x.name.Contains("Camera")).ToList();

                UpdateRootObject(ServerChildren, objectsOnly);

                //Update geometry
                UpdateRootObject(ServerGeometry, LocalModifications.geometries);

                UpdateRootObject(ServerMaterials, LocalModifications.materials);

                await firestoreDb.Collection(CollectionName).Document(DocumentName).SetAsync(ServerModel);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
                return;
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
                }
                else
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
            DocumentReference models = firestoreDb.Collection(CollectionName).Document(DocumentName);
            DocumentSnapshot snapshot = await models.GetSnapshotAsync();
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

        private static async void ReceiveChangesFromDB()
        {
            Rootobject RootObject = new Rootobject();
            try
            {
                DocumentReference docRef = firestoreDb.Collection("models").Document("test1");
                FirestoreChangeListener listener = docRef.Listen(snapshot =>
                {
                    Console.WriteLine("Callback received document snapshot.");

                    if (snapshot.Exists)
                    {
                        Console.WriteLine("Document data for {0} document:", snapshot.Id);

                        var st = Newtonsoft.Json.JsonConvert.SerializeObject(snapshot.ToDictionary());
                        RootObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Rootobject>(st);
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