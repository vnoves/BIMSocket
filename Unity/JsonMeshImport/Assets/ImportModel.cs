using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
        public class Rootobject
        {
            public Geometry[] geometries { get; set; }
            public Material[] materials { get; set; }
            public Metadata metadata { get; set; }
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
            public Child1[] children { get; set; }
            public Userdata userData { get; set; }
            public string material { get; set; }
        }

        public class Userdata
        {
            public string FasciaDepth { get; set; }
            public string RafterCut { get; set; }
            public string Image { get; set; }
            public string PhaseCreated { get; set; }
            public string PhaseDemolished { get; set; }
            public string WorkPlane { get; set; }
            public string Slope { get; set; }
            public string RoomBounding { get; set; }
            public string RelatedtoMass { get; set; }
            public string Thickness { get; set; }
            public string ExtrusionStart { get; set; }
            public string ExtrusionEnd { get; set; }
            public string Volume { get; set; }
            public string Area { get; set; }
            public string ReferenceLevel { get; set; }
            public string LevelOffset { get; set; }
            public string TypeDefaultThickness { get; set; }
            public string TypeTypeImage { get; set; }
            public string TypeAssemblyDescription { get; set; }
            public string TypeAssemblyCode { get; set; }
            public string TypeCost { get; set; }
            public string TypeHeatTransferCoefficientU { get; set; }
            public string TypeThermalResistanceR { get; set; }
            public string TypeThermalmass { get; set; }
            public string TypeAbsorptance { get; set; }
            public string TypeRoughness { get; set; }
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

        public class Child1
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
}
