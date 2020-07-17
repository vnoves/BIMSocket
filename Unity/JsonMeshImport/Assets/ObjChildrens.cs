using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using Assets;
using Newtonsoft.Json;
using UnityEngine.UIElements;
using UnityEditor;

namespace Assets
{
    public static class ObjChildrens
    {
        public static void create(List<Child> childrens, GameObject parentObject, List<GameObject> createdObj)
        {
            foreach(Child ChObj in childrens)
            {
                GameObject newObject = new GameObject(ChObj.name);
                newObject.transform.parent = parentObject.transform;

                if(ChObj.children.Length > 0)
                {
                    foreach(Children ch in ChObj.children)
                    {
                        foreach(GameObject go in createdObj)
                        {
                            if(ch.uuid.ToString() == go.name.ToString())
                            {         
                                string path = "Assets/Resources/" + ch.material + ".mat";

                                var myType = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Material)) as UnityEngine.Material;
                                if (myType != null)
                                {
                                    go.GetComponent<MeshRenderer>().material = myType;
                                }
                                go.transform.parent = newObject.transform;
                            }
                        }
                        
                    }
                }
            }
        }
    }
}
