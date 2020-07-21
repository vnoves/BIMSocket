using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMSocket
{
    public class RhinoToJsonPlugIn : Rhino.PlugIns.PlugIn

    {
        public RhinoToJsonPlugIn()
        {
            Instance = this;
        }

        ///<summary>Gets the only instance of the RhinoToJsonPlugIn plug-in.</summary>
        public static RhinoToJsonPlugIn Instance
        {
            get; private set;
        }

        // You can override methods here to change the plug-in behavior on
        // loading and shut down, add options pages to the Rhino _Option command
        // and mantain plug-in wide options in a document.
    }
}
