using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace Assets.Json
{
    public static class Import
    {
        public static void LoadJson(ref Rootobject importModel, string pathLocation)
        {
            using (StreamReader stream = new StreamReader(pathLocation))
            {
                string jsonString = stream.ReadToEnd();
                Debug.Log(jsonString);
                importModel = JsonConvert.DeserializeObject<Rootobject>(jsonString);
            }
        }
    }
}
