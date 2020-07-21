using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.DocObjects;
using RCva3c;

namespace BIMSocket
{
    [System.Runtime.InteropServices.Guid("6B5D0D30-4E11-461D-BA49-48EA58C711DB")]
    public class RhinoToJsonCommand : Command
    {
        public RhinoToJsonCommand()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static RhinoToJsonCommand Instance
        {
            get; private set;
        }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName
        {
            get { return "BIMSocket"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {

            MainForm mainForm = new MainForm(doc);
            mainForm.Show();
    
            return Result.Success;
        }



   
    }
}
