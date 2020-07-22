using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMSocket
{
    class ExportEvent : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            var _doc = app.ActiveUIDocument.Document;
            RevitManagement.SetCurrentDocument(_doc);
            RevitManagement.SetCurrentUIDocument(app.ActiveUIDocument);
            var changes = RevitManagement.ProcessLocalChanges();


            RevitManagement.changedElements = new List<ElementId>();
            MainForm.ClearChangedItems();

            if (changes == null)
            {
                return;
            }
            FireBaseConnection.SendChangesToDB(changes, null);

  
        }

        public string GetName()
        {
            return "External Event export changes";
        }
    }

    class ExportModelEvent : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            var _doc = app.ActiveUIDocument.Document;
            RevitManagement.SetCurrentDocument(_doc);
            RevitManagement.SetCurrentUIDocument(app.ActiveUIDocument);

            if (_doc.ActiveView.ViewType != ViewType.ThreeD)
            {
                var td = new TaskDialog("Wrong view type");
                td.MainInstruction = "Select a 3D View to start the app";
                td.Show();
                return;
            }

            RevitManagement.SetView3D(_doc.ActiveView as View3D);
            var CurrentRootObject = RevitManagement.ProcessAllModel();
            FireBaseConnection.SendModelToDB(CurrentRootObject, "models", _doc.Title);
        }

        public string GetName()
        {
            return "External Event Export model";
        }
    }

    class ReceiveChangesEvent : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            var _doc = app.ActiveUIDocument.Document;

            using (Transaction tx = new Transaction(_doc, "Updating Model"))
            {
                tx.Start();
                RevitManagement.ProcessReceivedChanges();
                tx.Commit();
                MainForm.ClearReceivedItems();
            }
        }

        public string GetName()
        {
            return "External Event apply changes in model";
        }
    }

}


