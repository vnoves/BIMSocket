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
            MainCommand._doc = app.ActiveUIDocument.Document;

            var changes = RevitManagement.ProcessLocalChanges();

            FireBaseConnection.SendChangesToDB(changes, null);

            RevitManagement.changedElements = new List<ElementId>();

            var changedElementsToRemove = MainForm.changedElements.ToList();
            foreach (var item in changedElementsToRemove)
            {
                MainForm.RemoveItem(item);
            }
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
            MainCommand._doc = app.ActiveUIDocument.Document;

            var ro = RevitManagement.ProcessAllModel();
            FireBaseConnection.SendModelToDB(ro, "models", MainCommand._doc.Title);
        }

        public string GetName()
        {
            return "External Event Export model";
        }
    }
}


