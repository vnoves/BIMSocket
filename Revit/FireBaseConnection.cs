using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace BIMSocket
{
    internal class FireBaseConnection
    {
        internal static List<ElementId> changedElements;
        internal static List<ElementId> deletedElements;

        public static FirebaseClient FirebaseClient { get; private set; }

        internal static bool Connect()
        {

            var url = "https://bimsocket.firebaseio.com/";
            var auth = "";
            var currentPath = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName + "\\credentials.txt";
            using (StreamReader streamReader = new StreamReader(currentPath))

            {
                auth = streamReader.ReadLine();
            }

            try
            {

                FirebaseClient = new FirebaseClient(url);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
                return false;
            }

        }


        internal static void ExportChanges()
        {

            if (!changedElements.Any() && !deletedElements.Any())
            {
                return; //Return if no changes
            }

            IsolateElementsIn3DView(changedElements);

            #region Format changed elements 

            Rootobject changed = ConvertChangedElementsToRootObject(changedElements);
            #endregion

            ResetsElementsIn3DView();

            #region Format deleted elements
            var deleted = "";
            if (deletedElements != null)
            {
                deleted =  ConvertDeletedElementsToJson(deletedElements);
            }

            #endregion

            SendChangesToDB(changed, deleted);

        }

        private static Rootobject ConvertChangedElementsToRootObject(ICollection<ElementId> elementsList)
        {

            var document = MainCommand.GetCurrentDocument();
            string jsonPath = ExportToJson(document, elementsList.ToList());

            var jsonString = "";
            using (StreamReader streamReader = new StreamReader(jsonPath))
            {
                jsonString = streamReader.ReadToEnd();
            }

            return FormatChanges(jsonString);

        }

        private static Rootobject FormatChanges(string jsonString)
        {

            
            return JsonConvert.DeserializeObject<Rootobject>(jsonString); ;
        }

        private static string ExportToJson(Document document, List<ElementId> ListOfElements)
        {
            RvtVa3c.Command command = new RvtVa3c.Command();
            View3D view3D = MainCommand.GetExportView3D();
            var jsonPath = Path.GetTempPath() + "BIMSocket.json";

            command.ExportView3D(view3D, jsonPath);
            return jsonPath;
        }

        private static bool IsolateElementsIn3DView(ICollection<ElementId> elementsList)
        {
            var view = MainCommand.GetExportView3D();
            var doc = MainCommand.GetCurrentDocument();
            //Reset any previous temporary hidden elements
            try
            {

                using (Transaction tx = new Transaction(doc))
            {

                tx.Start("Isolate");
                if (view.IsTemporaryHideIsolateActive())
                {
                    TemporaryViewMode tempView = TemporaryViewMode.TemporaryHideIsolate;
                    view.DisableTemporaryViewMode(tempView);
                }

                    view.IsolateElementsTemporary(elementsList);

                tx.Commit();

                    return true;
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            
        }


        private static bool ResetsElementsIn3DView()
        {
            var view = MainCommand.GetExportView3D();
            var doc = MainCommand.GetCurrentDocument();
            //Reset any previous temporary hidden elements
            try
            {

                using (Transaction tx = new Transaction(doc))
                {

                    tx.Start("Isolate");
                    if (view.IsTemporaryHideIsolateActive())
                    {
                        TemporaryViewMode tempView = TemporaryViewMode.TemporaryHideIsolate;
                        view.DisableTemporaryViewMode(tempView);
                    }
                    tx.Commit();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        private static string ConvertDeletedElementsToJson(ICollection<ElementId> deletedElements)
        {

            
            return FormatDeleted(deletedElements.Select(x=>x.IntegerValue));
        }

        private static string FormatDeleted(IEnumerable<int> enumerable)
        {
            return "";
        }


        private static async void SendChangesToDB(Rootobject changed, string deleted)
        {
            try
            {
                var obj = changed._object.children.Where(x => !x.name.Contains("Camera"));
                foreach (var item in obj)
                {
                    await FirebaseClient.Child("rootobject").Child("model1").Child("object").Child("children").Child(item.uuid).PutAsync(item);
                }

                var geometries = changed.geometries;
                foreach (var item in geometries)
                {
                    await FirebaseClient.Child("rootobject").Child("model1").Child("geometry").Child(item.uuid).PutAsync(item);
                }
           

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
                return ;
            }
        }

        private static async void ReceiveChangesFromDB()
        {
            try
            {
                
                var response = await FirebaseClient.Child("rootobject").OnceAsync<Rootobject>();

                //IReadOnlyCollection<FirebaseObject<object>> response = await FirebaseClient.Child("rootobject").OnceAsync<object>();
                //var rootObject = JsonConvert.DeserializeObject<Rootobject>(response.First().Object.ToString());

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
                return;
            }
        }
    }
}