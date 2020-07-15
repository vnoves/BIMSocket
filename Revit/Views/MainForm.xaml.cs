using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;

namespace BIMSocket
{
    /// <summary>
    /// Interaction logic for MainForm.xaml
    /// </summary>
    public partial class MainForm : Window
    {

        private ExternalCommandData p_commanddata;

        public Document _doc;

        public UIApplication uiApp;


        private readonly object _changedElementsLock = new object();
        internal static ObservableCollection<ElementId> _changedElements;

        public ObservableCollection<ElementId> ChangedElementsCollection
        {
            get { return _changedElements; }
            set
            {
                _changedElements = value;

                BindingOperations.EnableCollectionSynchronization(_changedElements, _changedElementsLock);
            }
        }


        private readonly object _receviedElementslock = new object();
        internal static ObservableCollection<ElementId> _receivedElements;

        public ObservableCollection<ElementId> ReceivedElementsCollection
        {
            get { return _receivedElements; }
            set
            {
                _receivedElements = value;
                BindingOperations.EnableCollectionSynchronization(_receivedElements, _receviedElementslock);
            }
        }


        /// <summary>
        /// Check if the suffix is a number or not
        /// </summary>
        /// <param name="cmddata_p"></param>
        public MainForm(ExternalCommandData cmddata_p)
        {
            this.DataContext = this;
            this.p_commanddata = cmddata_p;

            cleanVariables();

            InitializeComponent();

            this._doc = cmddata_p.Application.ActiveUIDocument.Document;
            bool connected = ConnectToDB();
            FireBaseConnection.ReceiveChangesFromDB();
            this.Closed += ClosingWindow;
            this.SendChangesList.ItemsSource = ChangedElementsCollection;
            this.ReceiveChangesList.ItemsSource = ReceivedElementsCollection;
        }

        private void cleanVariables()
        {
            ChangedElementsCollection.Clear();

            RevitManagement.changedElements = new List<ElementId>();


            ReceivedElementsCollection.Clear();
            RevitManagement.geometryChanges = new Dictionary<ElementId, Child>();

        }
        private void ClosingWindow(object sender, EventArgs e)
        {
            cleanVariables();
        }

        private bool ConnectToDB()
        {

            return FireBaseConnection.Connect("models", this._doc.Title);
        }

        public string projectName = App.NameSpaceNm;
        public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }

        private void Border_MouseDown
         (object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Mainbutton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SendChangesList.Items.Count >= 0)
            {
                App.ExportChangesExternalEvent.Raise();
            }

            if (this.ReceiveChangesList.Items.Count >= 0)
            {
                App.ReceiveChangesExternalEvent.Raise();
                
            }

        }

        private void SendModel_Click(object sender, RoutedEventArgs e)
        {
            App.ExportModelExternalEvent.Raise();
        }
    }
}
