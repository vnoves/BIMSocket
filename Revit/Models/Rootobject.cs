
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Documents;

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
    [FirestoreProperty(Name ="object")]

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
public class Child
{
    [FirestoreProperty]
    public string uuid { get; set; }
    [FirestoreProperty]
    public string name { get; set; }
    [FirestoreProperty]
    public string type { get; set; }
    [FirestoreProperty]
    public float[] matrix { get; set; }
    [FirestoreProperty]
    public Children[] children { get; set; }
    [FirestoreProperty]
    public Userdata userData { get; set; }
    [FirestoreProperty]
    public string material { get; set; }
}
[FirestoreData]
public class Userdata
{
    [FirestoreProperty]
    public string Level { get; set; }
    [FirestoreProperty]
    public string Height { get; set; }
    [FirestoreProperty]
    public string Width { get; set; }
    [FirestoreProperty]
    public string Mark { get; set; }
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
    public float[] matrix { get; set; }
    [FirestoreProperty]
    public string geometry { get; set; }
    [FirestoreProperty]
    public string material { get; set; }
}

[FirestoreData]
public class Geometry
{
  [FirestoreProperty] public string uuid { get; set; }
  [FirestoreProperty] public string type { get; set; }

    [JsonProperty("data")] [FirestoreProperty]  Data data { get; set; }
}

[FirestoreData]
public class Data
{
    [FirestoreProperty]
    public List<object> normals { get; set; }
    [FirestoreProperty]
    public List<int> faces { get; set; }
    [FirestoreProperty]
    public bool castShadow { get; set; }
    [FirestoreProperty]
    public bool receiveShadow { get; set; }
    [FirestoreProperty]
    public List<object> uvs { get; set; }
    [FirestoreProperty]
    public bool doubleSided { get; set; }

    [FirestoreProperty]
    public float scale { get; set; }
    [FirestoreProperty]
    public List<float> vertices { get; set; }
    [FirestoreProperty]
    public bool visible { get; set; }

}


[FirestoreData]
public class Material
{
  [FirestoreProperty] public string uuid { get; set; }
  [FirestoreProperty] public string name { get; set; }
  [FirestoreProperty] public string type { get; set; }
  [FirestoreProperty] public int color { get; set; }
  [FirestoreProperty] public int ambient { get; set; }
  [FirestoreProperty] public int emissive { get; set; }
  [FirestoreProperty] public int specular { get; set; }
  [FirestoreProperty] public int shininess { get; set; }
  [FirestoreProperty] public float opacity { get; set; }
  [FirestoreProperty] public bool transparent { get; set; }
  [FirestoreProperty] public bool wireframe { get; set; }
}

