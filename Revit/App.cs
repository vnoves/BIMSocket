#region Namespaces
using System;
using System.IO;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Events;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using System.Linq;
#endregion


namespace BIMSocket
{
    class App : IExternalApplication
    {

        static Type myType = typeof(App);
        static string nameSpaceNm = myType.Namespace;
        internal static ExternalEvent exEvent;
        internal static ExternalEvent exEventB;

        public static string NameSpaceNm

        {

            get { return nameSpaceNm; }

            set { nameSpaceNm = value; }

        }

        public Result OnStartup(UIControlledApplication application)
        {

            System.AppDomain currentDomain = System.AppDomain.CurrentDomain;

            currentDomain.AssemblyResolve += new ResolveEventHandler(currentDomain_AssemblyResolve);


            try
            {
                application.ControlledApplication.DocumentChanged +=
                    new EventHandler<Autodesk.Revit.DB.Events.DocumentChangedEventArgs>(documentChangedEventFillingListOfElements);
            }
            catch (Exception)
            {
                return Result.Failed;
            }

            // Get the absolut path of this assembly
            string ExecutingAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly(
               ).Location;

            // Create a ribbon panel
            RibbonPanel m_projectPanel = application.CreateRibbonPanel(
                NameSpaceNm);

            //Execute File location
            string fileLctn = NameSpaceNm + ".MainCommand";

            //Button
            PushButton pushButton = m_projectPanel.AddItem(new PushButtonData(
                    NameSpaceNm, NameSpaceNm, ExecutingAssemblyPath,
                    fileLctn)) as PushButton;

            //Add Help ToolTip 
            pushButton.ToolTip = NameSpaceNm;

            //Add long description 
            pushButton.LongDescription =
             "This addin helps you to ...";

            //Icon file location
            string iconFlLctn = NameSpaceNm + ".Resources.Icon.png";

            // Set the large image shown on button.
            pushButton.LargeImage = PngImageSource(
                iconFlLctn);

            // Get the location of the solution DLL
            string path = System.IO.Path.GetDirectoryName(
               System.Reflection.Assembly.GetExecutingAssembly().Location);

            // Combine path with \
            string newpath = Path.GetFullPath(Path.Combine(path, @"..\"));


            ContextualHelp contextHelp = new ContextualHelp(
                ContextualHelpType.Url,
                "https://google.com");

            // Assign contextual help to pushbutton
            pushButton.SetContextualHelp(contextHelp);

            // A new handler to handle request posting by the dialog
            ExportEvent handler = new ExportEvent();

            // External Event for the dialog to use (to post requests)
            exEvent = ExternalEvent.Create(handler);

            // A new handler to handle request posting by the dialog
            ExportModelEvent handlerB = new ExportModelEvent();

            // External Event for the dialog to use (to post requests)
            exEventB = ExternalEvent.Create(handlerB);

            return Result.Succeeded;

        }

        System.Reflection.Assembly currentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.Contains("Apis.Auth"))
            {
                string filename = Path.GetDirectoryName(
                  System.Reflection.Assembly
                    .GetExecutingAssembly().Location);

                filename = Path.Combine(filename,
                  "Google.Apis.Auth.dll");

                if (File.Exists(filename))
                {
                    return System.Reflection.Assembly
                      .LoadFrom(filename);
                }
            }
            return null;
        }


        private void documentChangedEventFillingListOfElements(object sender, DocumentChangedEventArgs e)
        {
            if (MainForm.changedElements == null)
            { return; }
            

            ElementClassFilter FamilyInstanceFilter = new ElementClassFilter(typeof(FamilyInstance));
            var addedElements = e.GetAddedElementIds(FamilyInstanceFilter);
            var deletedElements = e.GetDeletedElementIds();
            var modifiedElements = e.GetModifiedElementIds(FamilyInstanceFilter);
 
            if (RevitManagement.changedElements == null)
            {
                RevitManagement.changedElements = new List<ElementId>();
            }

            RevitManagement.changedElements.AddRange(addedElements);
            RevitManagement.changedElements.AddRange(modifiedElements);
            RevitManagement.deletedElements = deletedElements.ToList();
            RevitManagement.changedElements = RevitManagement.changedElements.Distinct().ToList();

            foreach (var item in RevitManagement.changedElements)
            {
                if (!MainForm.changedElements.Contains(item))
                {
                    MainForm.AddItem(item);
                }
                
            }
            
        }



        private System.Windows.Media.ImageSource PngImageSource(string embeddedPath)
        {
            // Get Bitmap from Resources folder
            Stream stream = this.GetType().Assembly.GetManifestResourceStream(embeddedPath);
            var decoder = new System.Windows.Media.Imaging.PngBitmapDecoder(stream,
                BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            return decoder.Frames[0];
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }
}



