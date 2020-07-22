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


        public static MainForm mainForm { get; private set; }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            mainForm = new MainForm(commandData);
            Process process = Process.GetCurrentProcess();

            RevitManagement.SetCurrentDocument(commandData.Application.ActiveUIDocument.Document);
            RevitManagement.SetCurrentUIDocument(commandData.Application.ActiveUIDocument);

            IntPtr h = process.MainWindowHandle;
            mainForm.Topmost = true;
            mainForm.Show(); //Changed to modeless
            return Result.Succeeded;
        }

    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class ExportCommand : IExternalCommand
    {
        static AddInId appId = new AddInId(new Guid("3256F49C-7F76-4734-8992-3F1CF468BE9B"));

        internal static Document _doc;
        internal static View3D view3DToExport;
        internal static UIDocument _uidoc;


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            _doc = commandData.Application.ActiveUIDocument.Document;
            RevitManagement.SetCurrentDocument(_doc);
            RevitManagement.SetCurrentUIDocument(commandData.Application.ActiveUIDocument);

            if (_doc.ActiveView.ViewType != ViewType.ThreeD)
            {
                var td = new TaskDialog("Wrong view type");
                td.MainInstruction = "Select a 3D View to start the app";
                td.Show();
                return Result.Cancelled;
            }

            RevitManagement.SetView3D((View3D)_doc.ActiveView);
            ConnectToDB();
            App.ExportModelExternalEvent.Raise();
            return Result.Succeeded;
        }


        private bool ConnectToDB()
        {

            return FireBaseConnection.Connect("models", _doc.Title);
        }
    }
}



