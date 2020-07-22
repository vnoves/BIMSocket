using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace RCva3c
{
    public class va3c_SceneCompiler
    {
        /// <summary>
        /// Generate final JSON for scene
        /// </summary>
        /// <param name="inElements">va3c Elements to add to the scene.</param>
        /// <returns></returns>
        public string GenerateSceneJson(List<Element> inElements)
        {
            if (inElements == null)
            {
                return null;
            }

            #region Input management

            List<string> inMeshGeometry = new List<string>();
            List<string> inLineGeometry = new List<string>();
            List<string> inViews = new List<string>();

            List<string> inMeshMaterial = new List<string>();
            List<string> inLineMaterial = new List<string>();

            List<string> inMeshLayer = new List<string>();
            List<string> inLineLayer = new List<string>();

            Dictionary<string, List<Element>> definitionLayers = new Dictionary<string, List<Element>>();

            foreach (Element e in inElements)
            {
                if (e == null) continue;
                string g = "";
                g = e.GeometryJson;

                if (e.Type != va3cElementType.Camera)
                {
                    string m = "";
                    m = e.Material.MaterialJson;

                    string layerName = "";
                    if (e.Layer == null) layerName = "Default";
                    else layerName = e.Layer.Name;

                    string l = "";
                    l = layerName;


                    if (e.Type == va3cElementType.Mesh)
                    {
                        inMeshGeometry.Add(g);
                        inMeshMaterial.Add(m);
                        inMeshLayer.Add(l);
                    }

                    if (e.Type == va3cElementType.Line)
                    {
                        inLineGeometry.Add(g);
                        inLineMaterial.Add(m);
                        inLineLayer.Add(l);
                    }



                    if (!definitionLayers.Keys.Contains(layerName))
                    {
                        List<Element> layerElements = new List<Element>();
                        definitionLayers.Add(layerName, layerElements);
                    }

                    definitionLayers[layerName].Add(e);


                }
                else
                {
                    inViews.Add(g);
                }
            }

            #endregion

            //compile geometry + materials into one JSON object with metadata etc.
            //https://raw.githubusercontent.com/mrdoob/three.js/master/examples/obj/blenderscene/scene.js
            //create json from lists of json:
            string outJSON = sceneJSON(inMeshGeometry, inMeshMaterial, inMeshLayer, inLineGeometry, inLineMaterial, inLineLayer, inViews, definitionLayers);
            outJSON = outJSON.Replace("OOO", "object");
            return outJSON;
        }

        private string sceneJSON(List<string> meshList, List<string> meshMaterialList, List<string> meshLayerList, List<string> linesList, List<string> linesMaterialList, List<string> lineLayerList, List<string> viewList, Dictionary<string, List<Element>> defLayers)
        {

            //create a dynamic object to populate
            dynamic jason = new ExpandoObject();

            //populate metadata object
            jason.metadata = new ExpandoObject();
            jason.metadata.version = 4.3;
            jason.metadata.type = "Object";
            jason.metadata.generator = "RCva3c_Rhinocommon_Exporter";

            int size = meshList.Count + linesList.Count;

            //populate mesh geometries:
            jason.geometries = new object[size];   //array for geometry - both lines and meshes
            jason.materials = new object[size];  //array for materials - both lines and meshes


            #region Mesh management
            int meshCounter = 0;
            Dictionary<string, object> MeshDict = new Dictionary<string, object>();
            Dictionary<string, va3cAttributesCatcher> attrDict = new Dictionary<string, va3cAttributesCatcher>();


            foreach (string m in meshList)
            {
                bool alreadyExists = false;
                //deserialize the geometry and attributes, and add them to our object
                va3cGeometryCatcher c = JsonConvert.DeserializeObject<va3cGeometryCatcher>(m);
                va3cAttributesCatcher ac = JsonConvert.DeserializeObject<va3cAttributesCatcher>(m);
                jason.geometries[meshCounter] = c;
                attrDict.Add(c.uuid, ac);


                //now that we have different types of materials, we need to know which catcher to call
                //use the va3cBaseMaterialCatcher class to determine a material's type, then call the appropriate catcher
                //object mc;
                va3cBaseMaterialCatcher baseCatcher = JsonConvert.DeserializeObject<va3cBaseMaterialCatcher>(meshMaterialList[meshCounter]);
                if (baseCatcher.type == "MeshFaceMaterial")
                {
                    va3cMeshFaceMaterialCatcher mc = JsonConvert.DeserializeObject<va3cMeshFaceMaterialCatcher>(meshMaterialList[meshCounter]);

                    foreach (var existingMaterial in jason.materials)
                    {
                        try
                        {
                            if (existingMaterial.type == "MeshFaceMaterial")
                            {
                                //check if all the properties match a material that already exists
                                if (mc.materials == existingMaterial.materials)
                                {
                                    mc.uuid = existingMaterial.uuid;
                                    alreadyExists = true;
                                    break;
                                }
                            }
                        }
                        catch { }
                    }
                    //only add it if it does not exist
                    if (!alreadyExists) jason.materials[meshCounter] = mc;
                    MeshDict.Add(c.uuid, mc.uuid);
                }
                if (baseCatcher.type == "MeshPhongMaterial")
                {
                    va3cMeshPhongMaterialCatcher mc = JsonConvert.DeserializeObject<va3cMeshPhongMaterialCatcher>(meshMaterialList[meshCounter]);

                    foreach (var existingMaterial in jason.materials)
                    {
                        try
                        {
                            if (existingMaterial.type == "MeshPhongMaterial")
                            {
                                //check if all the properties match a material that already exists
                                if (mc.color == existingMaterial.color && mc.ambient == existingMaterial.ambient && mc.emissive == existingMaterial.emissive
                                     && mc.side == existingMaterial.side && mc.opacity == existingMaterial.opacity && mc.shininess == existingMaterial.shininess
                                    && mc.specular == existingMaterial.specular && mc.transparent == existingMaterial.transparent && mc.wireframe == existingMaterial.wireframe)
                                {
                                    mc.uuid = existingMaterial.uuid;
                                    alreadyExists = true;
                                    break;
                                }
                            }
                        }
                        catch { }
                    }
                    //only add it if it does not exist
                    if (!alreadyExists) jason.materials[meshCounter] = mc;


                    MeshDict.Add(c.uuid, mc.uuid);
                }
                if (baseCatcher.type == "MeshLambertMaterial")
                {
                    va3cMeshLambertMaterialCatcher mc = JsonConvert.DeserializeObject<va3cMeshLambertMaterialCatcher>(meshMaterialList[meshCounter]);

                    foreach (var existingMaterial in jason.materials)
                    {
                        try
                        {
                            if (existingMaterial.type == "MeshLambertMaterial")
                            {
                                //check if all the properties match a material that already exists
                                if (mc.color == existingMaterial.color && mc.ambient == existingMaterial.ambient && mc.emissive == existingMaterial.emissive
                                    && mc.side == existingMaterial.side && mc.opacity == existingMaterial.opacity && mc.shading == existingMaterial.shading)
                                {
                                    mc.uuid = existingMaterial.uuid;
                                    alreadyExists = true;
                                    break;
                                }
                            }
                        }
                        catch
                        { }
                    }
                    //only add it if it does not exist
                    if (!alreadyExists) jason.materials[meshCounter] = mc;
                    MeshDict.Add(c.uuid, mc.uuid);
                }
                if (baseCatcher.type == "MeshBasicMaterial")
                {
                    va3cMeshBasicMaterialCatcher mc = JsonConvert.DeserializeObject<va3cMeshBasicMaterialCatcher>(meshMaterialList[meshCounter]);

                    foreach (var existingMaterial in jason.materials)
                    {
                        try
                        {
                            if (existingMaterial.type == "MeshBasicMaterial")
                            {
                                //check if all the properties match a material that already exists
                                if (
                                    mc.color == existingMaterial.color && mc.transparent == existingMaterial.transparent
                                    && mc.side == existingMaterial.side && mc.opacity == existingMaterial.opacity)
                                {
                                    mc.uuid = existingMaterial.uuid;
                                    alreadyExists = true;
                                    break;
                                }
                            }
                        }
                        catch
                        { }
                    }
                    //only add it if it does not exist
                    if (!alreadyExists) jason.materials[meshCounter] = mc;
                    MeshDict.Add(c.uuid, mc.uuid);
                }
                meshCounter++;

            }



            #endregion

            #region Line management
            //populate line geometries
            int lineCounter = meshCounter;
            int lineMaterialCounter = 0;
            Dictionary<string, object> LineDict = new Dictionary<string, object>();
            foreach (string l in linesList)
            {
                bool alreadyExists = false;
                //deserialize the line and the material
                va3cLineCatcher lc = JsonConvert.DeserializeObject<va3cLineCatcher>(l);
                va3cLineBasicMaterialCatcher lmc = JsonConvert.DeserializeObject<va3cLineBasicMaterialCatcher>(linesMaterialList[lineMaterialCounter]);
                //add the deserialized values to the jason object
                jason.geometries[lineCounter] = lc;


                foreach (var existingMaterial in jason.materials)
                {
                    try
                    {
                        if (existingMaterial.type == "LineBasicMaterial")
                        {
                            //check if all the properties match a material that already exists
                            if (
                                lmc.color == existingMaterial.color && lmc.linewidth == existingMaterial.linewidth
                                 && lmc.opacity == existingMaterial.opacity)
                            {
                                lmc.uuid = existingMaterial.uuid;
                                alreadyExists = true;
                                break;
                            }
                        }
                    }
                    catch
                    { }
                }
                //only add it if it does not exist
                if (!alreadyExists) jason.materials[meshCounter + lineMaterialCounter] = lmc;

                //populate dict to match up materials and lines
                LineDict.Add(lc.uuid, lmc.uuid);

                //increment counters
                lineCounter++;
                lineMaterialCounter++;
            }
            #endregion


            //make a new array that has the correct size according to the number of materials in the scene
            object[] myMaterials = jason.materials;
            myMaterials = myMaterials.Where(mat => mat != null).ToArray();
            jason.materials = myMaterials;

            #region Camera management
            //populate line geometries
            int viewCounter = 0;

            Dictionary<string, List<object>> viewDict = new Dictionary<string, List<object>>();
            foreach (string l in viewList)
            {
                //deserialize the line and the material
                va3cCameraCatcher lc = JsonConvert.DeserializeObject<va3cCameraCatcher>(l);

                List<object> viewSettings = new List<object>();
                viewSettings.Add(lc.eye);
                viewSettings.Add(lc.target);

                viewDict.Add(lc.name, viewSettings);

                //increment counters
                viewCounter++;

            }
            #endregion

            jason.OOO = new ExpandoObject();
            //create scene:
            jason.OOO.uuid = System.Guid.NewGuid();
            jason.OOO.type = "Scene";
            int[] numbers = new int[16] { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 };
            jason.OOO.matrix = numbers;
            jason.OOO.children = new object[meshList.Count + linesList.Count];
            jason.OOO.userData = new ExpandoObject();



            //create childern
            //loop over meshes and lines
            int i = 0;
            foreach (var g in MeshDict.Keys) //meshes
            {
                jason.OOO.children[i] = new ExpandoObject();
                jason.OOO.children[i].uuid = g + MeshDict[g];
                jason.OOO.children[i].name = "mesh" + g;
                jason.OOO.children[i].type = "Object3D";
                jason.OOO.children[i].geometry = g;
                jason.OOO.children[i].matrix = numbers;
                jason.OOO.children[i].userData = attrDict[g].userData;
                jason.OOO.children[i].children = new object[1];
                jason.OOO.children[i].children[0] = new ExpandoObject();
                jason.OOO.children[i].children[0].uuid = g + MeshDict[g];
                jason.OOO.children[i].children[0].name = "mesh" + i.ToString();
                jason.OOO.children[i].children[0].type = "Mesh";
                jason.OOO.children[i].children[0].geometry = g;
                jason.OOO.children[i].children[0].material = MeshDict[g];
                jason.OOO.children[i].children[0].matrix = numbers;

                i++;
            }
            int lineCount = 0;
            foreach (var l in LineDict.Keys)
            {
                jason.OOO.children[i] = new ExpandoObject();
                jason.OOO.children[i].uuid = Guid.NewGuid();
                jason.OOO.children[i].name = "line " + i.ToString();
                jason.OOO.children[i].type = "Line";
                jason.OOO.children[i].geometry = l;
                jason.OOO.children[i].material = LineDict[l];
                jason.OOO.children[i].matrix = numbers;
                jason.OOO.children[i].userData = new ExpandoObject();
                jason.OOO.children[i].userData.layer = lineLayerList[lineCount];
                i++;
                lineCount++;
            }

            jason.OOO.userData.views = new object[viewList.Count];
            int j = 0;
            foreach (var n in viewDict.Keys)
            {
                jason.OOO.userData.views[j] = new ExpandoObject();
                jason.OOO.userData.views[j].name = n;
                jason.OOO.userData.views[j].eye = viewDict[n][0];
                jason.OOO.userData.views[j].target = viewDict[n][1];

                j++;
            }

            jason.OOO.userData.layers = new object[defLayers.Keys.Count];
            int li = 0;
            foreach (var n in defLayers.Keys)
            {
                jason.OOO.userData.layers[li] = new ExpandoObject();
                jason.OOO.userData.layers[li].name = n;
                li++;
            }


            return JsonConvert.SerializeObject(jason);
        }

        private bool isJSONfile(string fileExtension)
        {
            if (fileExtension.ToLower() == ".js" ||
                fileExtension.ToLower() == ".json")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}