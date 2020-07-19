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
            Dictionary<GameObject, Child> SceneElements = new Dictionary<GameObject,Child>();
            foreach(Child ChObj in childrens)
            {

                GameObject newObject = new GameObject(ChObj.name);
                newObject.transform.parent = parentObject.transform;

                if (ChObj.children != null)
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
                var x = ChObj.matrix[12];
                if (x != 1)
                {
                    x = ChObj.matrix[12] ;
                }
                var y = ChObj.matrix[13];
                if (y != 1)
                {
                    y = ChObj.matrix[13];
                }
                var z = ChObj.matrix[14];
                if (z != 1)
                {
                    z = ChObj.matrix[14] / -1;
                }
                Vector3 temp = new Vector3(x, y, z);
                newObject.transform.position += temp;
            }
        }
    }
}
