using Autodesk.Revit.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Document = Autodesk.Revit.DB.Document;

namespace BIMSocket
{
    public class RevitManagement
    {
        internal static List<ElementId> changedElements;
        internal static List<ElementId> deletedElements;
        internal static Rootobject ProcessLocalChanges()
        {

            if (!changedElements.Any() && !deletedElements.Any())
            {
                return null; //Return if no changes
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

            return changed;

        }

        private static Rootobject ConvertChangedElementsToRootObject(ICollection<ElementId> changedElements)
        {

            var document = MainCommand.GetCurrentDocument();
            string jsonPath = ExportToJson(document, changedElements.ToList());

            var jsonString = "";
            using (StreamReader streamReader = new StreamReader(jsonPath))
            {
                jsonString = streamReader.ReadToEnd();
            }

            return FormatChanges(jsonString);

        }

        private static Rootobject FormatChanges(string jsonString)
        {
            //TODO: leave it here if changes are needed before export
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

        private static string FormatDeleted(IEnumerable<int> ElementIdsAsInteger)
        {
            //TODO leaved here to be formated before sending to db
            return JsonConvert.SerializeObject(ElementIdsAsInteger);
        }


      
    }
}