using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.DocObjects;
using RCva3c;

namespace RhinoToJson
{
    [System.Runtime.InteropServices.Guid("acaad0ad-a761-4798-a4c1-ef19eab4e933")]
    public class RhinoToJsonCommand : Command
    {
        public RhinoToJsonCommand()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static RhinoToJsonCommand Instance
        {
            get; private set;
        }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName
        {
            get { return "RhinoToJson"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            RhinoApp.WriteLine("The {0} will construct JSON representation of your scene.", EnglishName);

            Rhino.DocObjects.ObjRef[] objrefs;
            Result rc = RhinoGet.GetMultipleObjects("Select your scene objects",
                                                                false, Rhino.DocObjects.ObjectType.AnyObject, out objrefs);

            List<Element> allElements = new List<Element>();

           if(objrefs?.Length > 0)
            {
                foreach(var obj in objrefs)
                {
                    var material = obj.Object().GetMaterial(true);
                    var mesh = new va3c_Mesh();
                    Mesh finalMesh = new Mesh();
                    RCva3c.Material mat;
                    if (material != null)
                    {
                         mat = new va3c_MeshPhongMaterial().GeneratePhongMaterial(material.DiffuseColor, material.AmbientColor, material.EmissionColor, material.SpecularColor, material.Shine, 1 - material.Transparency);
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
                            allElements.Add(mesh.GenerateMeshElement(finalMesh, mat, new List<string> { ObjectType.Brep.ToString() }, new List<string> { ObjectType.Brep.ToString() }));
                            break;
                        case ObjectType.Brep:
                            var brep = obj.Brep();
                            var meshBrep = Mesh.CreateFromBrep(brep);
                            if(meshBrep?.Length > 0)
                            {
                                foreach(var m in meshBrep)
                                {
                                    finalMesh.Append(m);
                                }
                            }
                            allElements.Add(mesh.GenerateMeshElement(finalMesh, mat, new List<string> { "objectId" }, new List<string> { obj.ObjectId.ToString() }));
                            break;
                        case ObjectType.Mesh:
                            var msh = obj.Mesh();
                            allElements.Add(mesh.GenerateMeshElement(msh, mat, new List<string> { "objectId" }, new List<string> { obj.ObjectId.ToString() }));
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
                            allElements.Add(mesh.GenerateMeshElement(finalMesh, mat, new List<string> { "objectId" }, new List<string> { obj.ObjectId.ToString() }));
                            break;
                        case ObjectType.AnyObject:
                            break;
                        default:
                            break;
                    }
                }
            }

            var scenecompiler = new va3c_SceneCompiler();
            string resultatas = scenecompiler.GenerateSceneJson(allElements);
            Filewriter writer = new Filewriter();
            writer.SaveFileTo(new List<string> { resultatas });
            return Result.Success;
        }
    }
}
