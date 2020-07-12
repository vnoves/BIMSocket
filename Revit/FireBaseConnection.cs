
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

namespace BIMSocket
{
    internal class FireBaseConnection
    {

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


        private static async void SendChangesToDB(Rootobject changed, string deleted)
        {
            try
            {
                //Check new objects

                var obj = changed._object.children.Where(x => !x.name.Contains("Camera"));

                
                var model = firestoreDb.Collection("model1");


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
                return;
            }
        }

        private static async void ReceiveChangesFromDB()
        {
            try
            {



            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
                return;
            }
        }

    }
}