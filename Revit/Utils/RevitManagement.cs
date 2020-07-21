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
        private static Dictionary<ElementId, Material> materialChanges;

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
            ExportToJson();

            var jsonString = "";
            using (StreamReader streamReader = new StreamReader(Utils.LocalFiles.getJson3DPath()))
            {
                jsonString = streamReader.ReadToEnd();
            }

            return FormatChanges(jsonString);

        }

        internal static void ApplyMaterialChanges(Document doc)
        {
            foreach (KeyValuePair<ElementId, Material> change in materialChanges)
            {
                var element = doc.GetElement(change.Key) as Autodesk.Revit.DB.Material;
                int value = change.Value.color;
                var blue = Convert.ToByte((value >> 0) & 255);
                var green = Convert.ToByte((value >> 8) & 255);
                var red = Convert.ToByte((value >> 16) & 255);

 
                element.Color = new Color(red,green, blue);

            }
            materialChanges.Clear();
            MainCommand._uidoc.RefreshActiveView();
        }

        internal static void ApplyGeometryChanges(Document doc)
        {
            foreach (KeyValuePair<ElementId, Child> change in geometryChanges)
            {
                var element = doc.GetElement(change.Key);
                var matrix = change.Value.matrix;
                Transform transform = Transform.Identity;
                var feet = 0.00328084;
                var movement= new XYZ( - matrix[12]* feet,  matrix[14]* feet, matrix[13]* feet);
                //transform.BasisY = new XYZ(matrix[3], matrix[4], matrix[5]);
                //transform.BasisZ = new XYZ(matrix[6], matrix[7], matrix[8]);
                
                ElementTransformUtils.MoveElement(doc, change.Key, movement);
                //ElementTransformUtils.MoveElement(doc, change.Key, transform.BasisY);
                //ElementTransformUtils.MoveElement(doc, change.Key, transform.BasisZ);


            }
            geometryChanges.Clear();
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

        private static void ExportToJson()
        {
            RvtVa3c.Command command = new RvtVa3c.Command();
            View3D view3D = MainCommand.GetExportView3D();
            var jsonPath = Utils.LocalFiles.getJson3DPath();
            command.ExportView3D(view3D, jsonPath);
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
            var modifiedMaterials = GetModifiedMaterials(newRootObject.materials, currentRootObject);

            SaveReceivedGeometryChanges(modifiedGeometry);
            SaveReceivedMaterialChanges(modifiedMaterials, existingObjects);
            //TODO how to determ changes in geometry
        }

       

        private static void SaveReceivedGeometryChanges(List<Child> modifiedGeometry)
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

                        MainForm.AddReceivedItem(element.Id);
                    }
                    
                }
            }
            
        }

        private static void SaveReceivedMaterialChanges(List<Material> modifiedMaterial, List<Child> existingObjects)
        {
            materialChanges = new Dictionary<ElementId, Material>();
            //var materials = new FilteredElementCollector(MainCommand._doc)
            //    .OfClass(typeof(Autodesk.Revit.DB.Material))
            //    .ToElements();


            foreach (var item in modifiedMaterial)
            {

                var objectWithModifiedMaterial = existingObjects.Select(x => x.children.Where(c => c.material == item.uuid).FirstOrDefault()).FirstOrDefault();

                if (objectWithModifiedMaterial != null)
                {
                    Element element = MainCommand._doc.GetElement(objectWithModifiedMaterial.material);


                    materialChanges[element.Id] = item;

                    if (!MainForm._receivedElements.Contains(element.Id))
                    {

                        MainForm.AddReceivedItem(element.Id);
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

        private static List<Material> GetModifiedMaterials(List<Material> materials, Rootobject currentRootObject)
        {
            var updatedMaterials = new List<Material>();
            foreach (var material in materials)
            {
                var changedColor = currentRootObject.materials.Where(x => x.uuid == material.uuid)
                    .Where(y => y.color != material.color).FirstOrDefault();
                if (changedColor != null)
                {
                    updatedMaterials.Add(material);
                }
            }
            return updatedMaterials;

        }
    }
}