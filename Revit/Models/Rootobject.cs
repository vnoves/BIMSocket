
using Newtonsoft.Json;

public class Rootobject
{
    public Geometry[] geometries { get; set; }
    public Material[] materials { get; set; }
    public Metadata metadata { get; set; }

    [JsonProperty("Object")]
    public Object _object { get; set; }
}

public class Metadata
{
    public string type { get; set; }
    public float version { get; set; }
    public string generator { get; set; }
}

public class Object
{
    public string uuid { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public float[] matrix { get; set; }
    public Child[] children { get; set; }
}

public class Child
{
    public string uuid { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public float[] matrix { get; set; }
    public Children[] children { get; set; }
    public Userdata userData { get; set; }
    public string material { get; set; }
}

public class Userdata
{
    public string InstallDepthfromoutside { get; set; }
    public string Level { get; set; }
    public string SillHeight { get; set; }
    public string PhaseCreated { get; set; }
    public string PhaseDemolished { get; set; }
    public string Frame { get; set; }
    public string Glass { get; set; }
    public string Casement { get; set; }
    public string WindowCillInterior { get; set; }
    public string WindowCillExterior { get; set; }
    public string BottomHungCasement { get; set; }
    public string TopHungCasement { get; set; }
    public string CasementSwinginPlan { get; set; }
    public string CasementPivot { get; set; }
    public string RoughWidth { get; set; }
    public string RoughHeight { get; set; }
    public string Height { get; set; }
    public string Width { get; set; }
    public string Image { get; set; }
    public string Mark { get; set; }
    public string HeadHeight { get; set; }
    public string TypeAnalyticConstruction { get; set; }
    public string TypeDefineThermalPropertiesby { get; set; }
    public string TypeVisualLightTransmittance { get; set; }
    public string TypeSolarHeatGainCoefficient { get; set; }
    public string TypeThermalResistanceR { get; set; }
    public string TypeHeatTransferCoefficientU { get; set; }
    public string TypeOperation { get; set; }
    public string TypeFrameDepth { get; set; }
    public string TypeFrameDepthunder { get; set; }
    public string TypeFrameDepthover { get; set; }
    public string TypeFrameWidth { get; set; }
    public string TypeCasementDepth { get; set; }
    public string TypeCasementWidth { get; set; }
    public string TypeWallClosure { get; set; }
    public string TypeDescription { get; set; }
    public string TypeTypeImage { get; set; }
    public string TypeCost { get; set; }
    public string TypeTypeMark { get; set; }
    public string revit_id { get; set; }
    public string ViewName { get; set; }
    public string Dependency { get; set; }
    public string DetailLevel { get; set; }
    public string PartsVisibility { get; set; }
    public string CropView { get; set; }
    public string CropRegionVisible { get; set; }
    public string FarClipActive { get; set; }
    public string FarClipOffset { get; set; }
    public string PhaseFilter { get; set; }
    public string Phase { get; set; }
    public string ScopeBox { get; set; }
    public string Discipline { get; set; }
    public string DefaultAnalysisDisplayStyle { get; set; }
    public string LockedOrientation { get; set; }
    public string ProjectionMode { get; set; }
    public string EyeElevation { get; set; }
    public string TargetElevation { get; set; }
    public string CameraPosition { get; set; }
    public string SectionBox { get; set; }
    public string None { get; set; }
    public string SunPath { get; set; }
    public string TypeNewviewsaredependentontemplate { get; set; }
    public string TypeCoarsePocheMaterial { get; set; }
    public string ViewScale { get; set; }
    public string ScaleValue1 { get; set; }
    public string DetailNumber { get; set; }
    public string SheetNumber { get; set; }
    public string SheetName { get; set; }
    public string RotationonSheet { get; set; }
    public string AnnotationCrop { get; set; }
    public string ShowHiddenLines { get; set; }
}

public class Children
{
    public string uuid { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public float[] matrix { get; set; }
    public string geometry { get; set; }
    public string material { get; set; }
}

public class Geometry
{
    public string uuid { get; set; }
    public string type { get; set; }
    public Data data { get; set; }
}

public class Data
{
    public float[] vertices { get; set; }
    public object[] normals { get; set; }
    public object[] uvs { get; set; }
    public int[] faces { get; set; }
    public float scale { get; set; }
    public bool visible { get; set; }
    public bool castShadow { get; set; }
    public bool receiveShadow { get; set; }
    public bool doubleSided { get; set; }
}

public class Material
{
    public string uuid { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public int color { get; set; }
    public int ambient { get; set; }
    public int emissive { get; set; }
    public int specular { get; set; }
    public int shininess { get; set; }
    public float opacity { get; set; }
    public bool transparent { get; set; }
    public bool wireframe { get; set; }
}

