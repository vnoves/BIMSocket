using BIMSocket.Models;
using Newtonsoft.Json;
using RCva3c;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Input;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMSocket.Utils
{
    class RhinoManagement
    {
        private static Rootobject _CurrentModel;
        internal static List<string> changedElements;
        internal static Dictionary<string, Child> geometryChanges;

        //internal static void ProcessRemoteChanges(Rootobject rootObject)
        //{
        //    throw new NotImplementedException();
        //}


        //internal static Rootobject ConvertModelToRootObject()
        //{
        //    ExportToJson();
        //    var jsonString = "";
        //    jsonString = FileManager.ReadJsonFile();

        //    try
        //    {
        //        var e = JsonConvert.DeserializeObject<Rootobject>(jsonString);
        //        return e;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}

        internal static string ConvertModelToString()
        {
            ExportToJson(null);
            var jsonString = "";
            jsonString = FileManager.ReadJsonFile();
            return jsonString;
            
        }



        private static void ExportToJson(List<string> guids)
        {


            var doc = Rhino.RhinoDoc.ActiveDoc;
           ObjectEnumeratorSettings settings = new ObjectEnumeratorSettings();
            settings.ActiveObjects = true; 
            List<Guid> ids = new List<Guid>();
            var objrefs =  doc.Objects.GetObjectList(settings);


            List<Element> allElements = new List<Element>();

 
            foreach (var rhinoobj in objrefs)
                {
                    var obj = new ObjRef(rhinoobj.Id);
                    var material = rhinoobj.GetMaterial(true);
                    var mesh = new va3c_Mesh();
                    Mesh finalMesh = new Mesh();
                    var parameterNames = new List<string> { "objectId" };
                    var parameterValues = new List<string> { obj.ObjectId.ToString() };
                    RCva3c.Material mat;
                    if (material != null)
                    {
                        mat = new va3c_MeshPhongMaterial().GeneratePhongMaterial(material.Id, material.DiffuseColor, material.AmbientColor, material.EmissionColor, material.SpecularColor, material.Shine, 1 - material.Transparency);

                    }
                    else
                    {
                        mat = new va3c_MeshBasicMaterial().GenerateMaterial(System.Drawing.Color.White, 1);
                    }

                    switch (obj.Geometry().ObjectType)
                    {
                        case ObjectType.None:
                            break;
                        case ObjectType.Point:
                            break;
                        case ObjectType.PointSet:
                            break;
                        case ObjectType.Curve:
                            break;
                        case ObjectType.Surface:
                            var srf = obj.Surface();
                            var meshSrf = Mesh.CreateFromBrep(srf.ToBrep());
                            if (meshSrf?.Length > 0)
                            {
                                foreach (var m in meshSrf)
                                {
                                    finalMesh.Append(m);
                                }
                            }
                            allElements.Add(mesh.GenerateMeshElement(finalMesh, mat, obj.ObjectId, parameterNames, parameterValues));
                            break;
                        case ObjectType.Brep:
                            var brep = obj.Brep();
                            var meshBrep = Mesh.CreateFromBrep(brep);
                            if (meshBrep?.Length > 0)
                            {
                                foreach (var m in meshBrep)
                                {
                                    finalMesh.Append(m);
                                }
                            }
                            allElements.Add(mesh.GenerateMeshElement(finalMesh, mat, obj.ObjectId, parameterNames, parameterValues));
                            break;
                        case ObjectType.Mesh:
                            var msh = obj.Mesh();
                            allElements.Add(mesh.GenerateMeshElement(msh, mat, obj.ObjectId, parameterNames, parameterValues));
                            break;
                        case ObjectType.Light:
                            break;
                        case ObjectType.Annotation:
                            break;
                        case ObjectType.InstanceDefinition:
                            break;
                        case ObjectType.InstanceReference:
                            break;
                        case ObjectType.TextDot:
                            break;
                        case ObjectType.Grip:
                            break;
                        case ObjectType.Detail:
                            break;
                        case ObjectType.Hatch:
                            break;
                        case ObjectType.MorphControl:
                            break;
                        case ObjectType.BrepLoop:
                            break;
                        case ObjectType.PolysrfFilter:
                            break;
                        case ObjectType.EdgeFilter:
                            break;
                        case ObjectType.PolyedgeFilter:
                            break;
                        case ObjectType.MeshVertex:
                            break;
                        case ObjectType.MeshEdge:
                            break;
                        case ObjectType.MeshFace:
                            break;
                        case ObjectType.Cage:
                            break;
                        case ObjectType.Phantom:
                            break;
                        case ObjectType.ClipPlane:
                            break;
                        case ObjectType.Extrusion:

                            var extruction = obj.Brep();
                            var meshExtruction = Mesh.CreateFromBrep(extruction);
                            if (meshExtruction?.Length > 0)
                            {
                                foreach (var m in meshExtruction)
                                {
                                    finalMesh.Append(m);
                                }
                            }
                            allElements.Add(mesh.GenerateMeshElement(finalMesh, mat, obj.ObjectId, parameterNames, parameterValues ));
                            break;
                        case ObjectType.AnyObject:
                            break;
                        default:
                            break;
                    }
                }
   

            var scenecompiler = new va3c_SceneCompiler();
            string resultatas = scenecompiler.GenerateSceneJson(allElements);
            FileManager writer = new FileManager();
            writer.SaveFileTo(new List<string> { resultatas });
        }

        internal static string ConvertChangesToString()
        {
            ExportToJson(changedElements);
            var jsonString = "";
            jsonString = FileManager.ReadJsonFile();

            changedElements.Clear();

            return jsonString;
        }

        internal static void SaveCurrentModel(Rootobject obj)
        {
            _CurrentModel = obj;
        }

        internal static void ProcessRemoteChanges(Rootobject newModel)
        {
            var modifiedGeometry = GetModifiedGeometry(_CurrentModel, newModel);
            foreach (var item in modifiedGeometry)
            {

                MainForm.AddReceivedItem(item.userData.objectId);

                geometryChanges[item.userData.objectId] =  item;
            }

        }

        public static void ApplyChanges()
        {
            var doc = Rhino.RhinoDoc.ActiveDoc;
            foreach (var item in geometryChanges)
            {
                MoveObject(doc, item.Value);
               
            }
            geometryChanges.Clear();

        }

        private static void MoveObject(RhinoDoc doc, Child item)
        {
            var guid = new Guid(item.userData.objectId);
            var obj = new ObjRef(guid);
            var matrix = item.matrix;
            Vector3d vec = new Vector3d(-matrix[12], matrix[14], matrix[13]);
            var xf = Rhino.Geometry.Transform.Translation(vec);

            Guid id = doc.Objects.Transform(obj, xf, true);
            if (id != Guid.Empty)
            {
                doc.Views.Redraw();
            }
        }

        private static List<Child> GetModifiedGeometry(Rootobject currentRootObject, Rootobject newModel)
        {
            var identity = new int[] { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 };

            var objectsWithChanges = newModel._object.children.Where(x => !x.matrix.SequenceEqual(identity));

            return objectsWithChanges.ToList();

        }

    }
}
