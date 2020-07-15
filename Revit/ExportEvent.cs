﻿using Autodesk.Revit.DB;
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

            MainForm._changedElements.Clear();
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

            var CurrentRootObject = RevitManagement.ProcessAllModel();
            FireBaseConnection.SendModelToDB(CurrentRootObject, "models", MainCommand._doc.Title);
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
            MainCommand._doc = app.ActiveUIDocument.Document;
            using (Transaction tx = new Transaction(MainCommand._doc,"Updating Model"))
            {
                tx.Start();
                RevitManagement.ApplyGeometryChanges(MainCommand._doc);

                tx.Commit();
                MainForm._receivedElements.Clear();
            }
        }

        public string GetName()
        {
            return "External Event apply changes in model";
        }
    }

}


