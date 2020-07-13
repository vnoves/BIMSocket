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
            return "External Event ClashDetective";
        }
    }
}


