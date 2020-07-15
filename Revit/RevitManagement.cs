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
        internal static Dictionary<ElementId, Child> geometryChanges;

        public static Rootobject CurrentRootObject { get; internal set; }

        internal static Rootobject ProcessAllModel()
        {
            return ConvertModelToRootObject();
        }

        internal static Rootobject ProcessLocalChanges()
        {

            if (!changedElements.Any() && !deletedElements.Any())
            {
                return null; //Return if no changes
            }

            IsolateElementsIn3DView(changedElements);

            #region Format changed elements 

            Rootobject changed = ConvertModelToRootObject();
            #endregion

            ResetsElementsIn3DView();

            #region Format deleted elements
            var deleted = "";
            if (deletedElements != null)
            {
                deleted = ConvertDeletedElementsToJson(deletedElements);
            }

            #endregion

            return changed;

        }



        private static Rootobject ConvertModelToRootObject()
        {

            var document = MainCommand.GetCurrentDocument();
            string jsonPath = ExportToJson(document);

            var jsonString = "";
            using (StreamReader streamReader = new StreamReader(jsonPath))
            {
                jsonString = streamReader.ReadToEnd();
            }

            return FormatChanges(jsonString);

        }

        internal static void ApplyGeometryChanges(Document doc)
        {
            foreach (KeyValuePair<ElementId, Child> change in geometryChanges)
            {
                var element = doc.GetElement(change.Key);
                var matrix = change.Value.matrix;
                Transform transform = Transform.Identity;
                transform.BasisX = new XYZ(matrix[0], matrix[1], matrix[2]);
                transform.BasisY = new XYZ(matrix[3], matrix[4], matrix[5]);
                transform.BasisZ = new XYZ(matrix[6], matrix[7], matrix[8]);
                
                ElementTransformUtils.MoveElement(doc, change.Key, transform.BasisX);
                ElementTransformUtils.MoveElement(doc, change.Key, transform.BasisY);
                ElementTransformUtils.MoveElement(doc, change.Key, transform.BasisZ);


            }
            
        }

        private static Rootobject FormatChanges(string jsonString)
        {
            //TODO: leave it here if changes are needed before export
            try
            {
                var e = JsonConvert.DeserializeObject<Rootobject>(jsonString);
                return e;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static string ExportToJson()
        {
            RvtVa3c.Command command = new RvtVa3c.Command();
            View3D view3D = MainCommand.GetExportView3D();
            var jsonPath = Path.GetTempPath() + "BIMSocket.json";

            command.ExportView3D(view3D, jsonPath);
            return jsonPath;
        }

        private static string ExportToJson(Document documents)
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

            return FormatDeleted(deletedElements.Select(x => x.IntegerValue));
        }

        private static string FormatDeleted(IEnumerable<int> ElementIdsAsInteger)
        {
            //TODO leaved here to be formated before sending to db
            return JsonConvert.SerializeObject(ElementIdsAsInteger);
        }

        internal static void ProcessRemoteChanges(Rootobject newRootObject)
        {
            if (CurrentRootObject != null)
            {
                CompareRootObjects(newRootObject, CurrentRootObject);
            }
            CurrentRootObject = newRootObject;
        }

        private static void CompareRootObjects(Rootobject newRootObject, Rootobject currentRootObject)
        {
            var currentObjectsUuid = currentRootObject._object.children.Select(x => x.uuid);
            var newObjects = newRootObject._object.children.Where(x => !currentObjectsUuid.Contains(x.uuid)).ToList();
            var existingObjects = newRootObject._object.children.Where(x => currentObjectsUuid.Contains(x.uuid)).ToList();
            var modifiedGeometry = GetModifiedGeometry(existingObjects, currentRootObject);

            SaveReceivedChanges(modifiedGeometry);
            //TODO how to determ changes in geometry
        }

        private static void SaveReceivedChanges(List<Child> modifiedGeometry)
        {
            geometryChanges = new Dictionary<ElementId, Child>();
            foreach (var item in modifiedGeometry)
            {
                var element = MainCommand._doc.GetElement(item.uuid);
                if (element!= null)
                {
                    geometryChanges[element.Id] = item;
                    if (!MainForm._receivedElements.Contains(element.Id))
                    {
                        MainForm._receivedElements.Add(element.Id);
                    }
                    
                }
            }
            
        }

        private static List<Child> GetModifiedGeometry(List<Child> existingObjects, Rootobject currentRootObject)
        {
            var identity = new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f };

            var geometryChanged = existingObjects.Where(x => !x.matrix.SequenceEqual(identity)).ToList();
            return geometryChanged;

        }
    }
}