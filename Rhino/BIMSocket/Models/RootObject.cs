using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMSocket.Models
{
    [FirestoreData]
    public class Rootobject
    {
        [FirestoreProperty]

        public List<Geometry> geometries { get; set; }

        [FirestoreProperty]
        public List<Material> materials { get; set; }
        [FirestoreProperty]

        public Metadata metadata { get; set; }

        [JsonProperty("object")]
        [FirestoreProperty(Name = "object")]

        public Object _object { get; set; }
    }
    [FirestoreData]
    public class Metadata
    {
        [FirestoreProperty]
        public string type { get; set; }
        [FirestoreProperty]
        public float version { get; set; }

        [FirestoreProperty]
        public string generator { get; set; }
    }
    [FirestoreData]
    public class Object
    {
        [FirestoreProperty]
        public string uuid { get; set; }
        [FirestoreProperty]
        [JsonProperty("name")]
        public string name { get; set; }
        [FirestoreProperty]
        public string type { get; set; }
        [FirestoreProperty]
        public float[] matrix { get; set; }

        [JsonProperty("children")]
        [FirestoreProperty]
        public List<Child> children { get; set; }
    }
    [FirestoreData]
    public class Userdata
    {
        [FirestoreProperty]
        public object[] views { get; set; }
        [FirestoreProperty]
        public Layer[] layers { get; set; }
        [FirestoreProperty]
        public string layer { get; set; }

        [FirestoreProperty(Name = "objectId")]
        [JsonProperty("objectId")]
        public string objectId { get; set; }
    }
    [FirestoreData]
    public class Layer
    {
        [FirestoreProperty]
        public string name { get; set; }
    }
    [FirestoreData]
    public class Child
    {
        [FirestoreProperty]
        public string uuid { get; set; }
        [FirestoreProperty]
        [JsonProperty("name")]
        public string name { get; set; }
        [FirestoreProperty]
        public string type { get; set; }
        [FirestoreProperty]
        public string geometry { get; set; }
        [FirestoreProperty]
        public int[] matrix { get; set; }
        [FirestoreProperty]
        public Userdata userData { get; set; }
        [FirestoreProperty]
        public Children[] children { get; set; }
    }


    [FirestoreData]
    public class Children
    {
        [FirestoreProperty]
        public string uuid { get; set; }
        [FirestoreProperty]
        public string name { get; set; }
        [FirestoreProperty]
        public string type { get; set; }
        [FirestoreProperty]
        public string geometry { get; set; }
        [FirestoreProperty]
        public string material { get; set; }
        [FirestoreProperty]
        public int[] matrix { get; set; }
    }
    [FirestoreData]
    public class Geometry
    {
        [FirestoreProperty] public string uuid { get; set; }
        [FirestoreProperty] public string type { get; set; }
        [JsonProperty("data")] [FirestoreProperty] public Data data { get; set; }
    }
    [FirestoreData]
    public class Data
    {
        [FirestoreProperty] public float[] vertices { get; set; }
        [FirestoreProperty] public int[] faces { get; set; }
        [FirestoreProperty] public object[] normals { get; set; }
        [FirestoreProperty] public object[] uvs { get; set; }
        [FirestoreProperty] public int scale { get; set; }
        [FirestoreProperty] public bool visible { get; set; }
        [FirestoreProperty] public bool castShadow { get; set; }
        [FirestoreProperty] public bool receiveShadow { get; set; }
        [FirestoreProperty] public bool doubleSided { get; set; }
    }
    [FirestoreData]
    public class Material
    {
        [FirestoreProperty] public string uuid { get; set; }
        [FirestoreProperty] public string type { get; set; }
        [FirestoreProperty] public string color { get; set; }
        [FirestoreProperty] public string ambient { get; set; }
        [FirestoreProperty] public string emissive { get; set; }
        [FirestoreProperty] public string specular { get; set; }
        [FirestoreProperty] public float shininess { get; set; }
        [FirestoreProperty] public float opacity { get; set; }
        [FirestoreProperty] public bool transparent { get; set; }
        [FirestoreProperty] public bool wireframe { get; set; }
        [FirestoreProperty] public int side { get; set; }
    }

}
