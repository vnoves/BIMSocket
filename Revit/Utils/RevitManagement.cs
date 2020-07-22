using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
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
        public static Rootobject CurrentRootObject { get; internal set; }
        public static View3D _view3D { get; private set; }

        internal static List<ElementId> changedElements;
        internal static List<ElementId> deletedElements;
        internal static Dictionary<ElementId, Child> geometryChanges;
        private static Document _doc { get; set; }
        private static UIDocument _uidoc;
        private static Dictionary<ElementId, Material> materialChanges;

        internal static void SetCurrentUIDocument(UIDocument activeUIDocument)
        {
            _uidoc = activeUIDocument;
        }

        internal static void SetCurrentDocument(Document doc)
        {
            _doc = doc;
        }

        internal static void SetView3D(View3D view3D)
        {
            _view3D = view3D;
        }

        /// <summary>
        /// Process all model into a Rootoject
        /// </summary>
        /// <returns></returns>
        internal static Rootobject ProcessAllModel()
        {
            return ConvertModelToRootObject();
        }

        /// <summary>
        /// Process only changed elements
        /// </summary>
        /// <returns></returns>
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

        internal static void ProcessReceivedChanges()
        {
            ApplyMaterialChanges();
            ApplyGeometryChanges();
        }

        internal static void CompareAndUpdateCurrentModel(Rootobject newRootObject)
        {
            if (CurrentRootObject != null)
            {
                CompareRootObjects(newRootObject, CurrentRootObject);
            }
            CurrentRootObject = newRootObject;
        }



        #region Export Json
        private static void ExportToJson()
        {
            RvtVa3c.Command command = new RvtVa3c.Command();
            View3D view3D = _doc.ActiveView as View3D;
            var jsonPath = Utils.LocalFiles.getJson3DPath();
            command.ExportView3D(view3D, jsonPath);
        }

        private static Rootobject ConvertModelToRootObject()
        {

            ExportToJson();

            var jsonString = "";
            using (StreamReader streamReader = new StreamReader(Utils.LocalFiles.getJson3DPath()))
            {
                jsonString = streamReader.ReadToEnd();
            }

            return FormatChanges(jsonString);

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

        private static bool IsolateElementsIn3DView(ICollection<ElementId> elementsList)
        {
            var view = _view3D;
            var doc = _doc;
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
            var view = _view3D;
            var doc = _doc;
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

        #endregion




        private static string ConvertDeletedElementsToJson(ICollection<ElementId> deletedElements)
        {

            return FormatDeleted(deletedElements.Select(x => x.IntegerValue));
        }

        private static string FormatDeleted(IEnumerable<int> ElementIdsAsInteger)
        {
            //TODO leaved here to be formated before sending to db
            return JsonConvert.SerializeObject(ElementIdsAsInteger);
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

        private static void ApplyMaterialChanges()
        {
            foreach (KeyValuePair<ElementId, Material> change in materialChanges)
            {
                var element = _doc.GetElement(change.Key) as Autodesk.Revit.DB.Material;
                int value = change.Value.color;
                var blue = Convert.ToByte((value >> 0) & 255);
                var green = Convert.ToByte((value >> 8) & 255);
                var red = Convert.ToByte((value >> 16) & 255);


                element.Color = new Color(red, green, blue);

            }
            materialChanges.Clear();
            _uidoc.RefreshActiveView();
        }

        private static void ApplyGeometryChanges()
        {
            foreach (KeyValuePair<ElementId, Child> change in geometryChanges)
            {
                var element = _doc.GetElement(change.Key);
                var matrix = change.Value.matrix;
                Transform transform = Transform.Identity;
                var feet = 0.00328084;
                var movement = new XYZ(-matrix[12] * feet, matrix[14] * feet, matrix[13] * feet);
                //transform.BasisY = new XYZ(matrix[3], matrix[4], matrix[5]);
                //transform.BasisZ = new XYZ(matrix[6], matrix[7], matrix[8]);

                ElementTransformUtils.MoveElement(_doc, change.Key, movement);
                //ElementTransformUtils.MoveElement(doc, change.Key, transform.BasisY);
                //ElementTransformUtils.MoveElement(doc, change.Key, transform.BasisZ);


            }
            geometryChanges.Clear();
        }

        private static void SaveReceivedGeometryChanges(List<Child> modifiedGeometry)
        {
            geometryChanges = new Dictionary<ElementId, Child>();
            foreach (var item in modifiedGeometry)
            {
                var element = _doc.GetElement(item.uuid);
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
                    Element element = _doc.GetElement(objectWithModifiedMaterial.material);


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