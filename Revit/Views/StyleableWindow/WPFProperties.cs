using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMSocket
{
    public class WPFProperties
    {
        public static string projectVersion = CommonAssemblyInfo.Number;
        public static string ProjectVersion
        {
            get { return projectVersion; }
            set { projectVersion = value; }
        }

        //Form Tooltips
        public static string clsButtonToolTip = "Close";

        public static string TtlButtonToolTip = "App's website";

        public static string MnButtonToolTip = "Apply";

        public static string ScdttonToolTip = "Button";

        //Form Texts
        public static string MnButtonText = "Apply";

        public static string ScdButtonText = "Button";

        //TextBox Texts
        public static string TboxText = "ABC";
    }
}
