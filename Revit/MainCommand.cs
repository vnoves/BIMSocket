#region Namespaces
using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Diagnostics;
#endregion


namespace BIMSocket
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class MainCommand : IExternalCommand
    {
        static AddInId appId = new AddInId(new Guid("3256F49C-7F76-4734-8992-3F1CF468BE9B"));

        internal static Document _doc;
        internal static View3D view3DToExport;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            MainForm mainForm = new MainForm(commandData);
            Process process = Process.GetCurrentProcess();

            _doc = commandData.Application.ActiveUIDocument.Document;

            if (_doc.ActiveView.ViewType != ViewType.ThreeD)
            {
                var td = new TaskDialog("Wrong view type");
                td.MainInstruction =  "Select a 3D View to start the app";
                td.Show();
                return Result.Cancelled;

            }
            view3DToExport = (View3D) _doc.ActiveView;
            IntPtr h = process.MainWindowHandle;
            mainForm.Topmost = true;
            mainForm.Show(); //Changed to modeless
            return Result.Succeeded;
        }

        internal static Document GetCurrentDocument()
        {
            return _doc;
        }

        internal static View3D GetExportView3D()
        {
            return view3DToExport;
        }

        internal static UIDocument GetCurrentUIDocument()
        {
            throw new NotImplementedException();
        }

        internal static UIApplication GetCurrentUIApplication()
        {
            throw new NotImplementedException();
        }
    }
}



