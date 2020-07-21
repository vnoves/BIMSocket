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
using System.Runtime.CompilerServices;
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


        private static object _changedElementsLock = new object();

        internal static ObservableCollection<ElementId> _changedElements;

        private static object _receviedElementslock = new object();

        internal static ObservableCollection<ElementId> _receivedElements;



        /// <summary>
        /// Check if the suffix is a number or not
        /// </summary>
        /// <param name="cmddata_p"></param>
        public MainForm(ExternalCommandData cmddata_p)
        {
            this.DataContext = this;
            this.p_commanddata = cmddata_p;

            InitializeComponent();

            _changedElements = new ObservableCollection<ElementId>();
            BindingOperations.EnableCollectionSynchronization(_changedElements, _changedElementsLock);
            _receivedElements = new ObservableCollection<ElementId>();
            BindingOperations.EnableCollectionSynchronization(_receivedElements, _receviedElementslock);

            cleanVariables();

            this._doc = cmddata_p.Application.ActiveUIDocument.Document;
            bool connected = ConnectToDB();
            FireBaseConnection.ReceiveChangesFromDB();
            this.Closed += ClosingWindow;
            this.SendChangesList.ItemsSource = _changedElements;
            this.ReceiveChangesList.ItemsSource = _receivedElements;
        }

        private void cleanVariables()
        {

            _changedElements.Clear();

            RevitManagement.changedElements = new List<ElementId>();

            _receivedElements.Clear();

            RevitManagement.geometryChanges = new Dictionary<ElementId, Child>();

        }

        private void ClosingWindow(object sender, EventArgs e)
        {
            cleanVariables();
        }


        public static void AddChangedItem(ElementId elementId)
        {
            lock (_changedElementsLock)
            {
                _changedElements.Add(elementId);
            }

        }

        public static void ClearChangedItems()
        {
            lock (_changedElementsLock)
            {
                _changedElements.Clear();
            }

        }

        public static void AddReceivedItem(ElementId elementId)
        {
            lock (_receviedElementslock)
            {
                _receivedElements.Add(elementId);
            }

        }
        public static void ClearReceivedItems()
        {
            lock (_receviedElementslock)
            {
                _receivedElements.Clear();
            }

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



        private void SendModel_Click(object sender, RoutedEventArgs e)
        {
            App.ExportModelExternalEvent.Raise();
        }



        private void Receive_Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.ReceiveChangesList.Items.Count >= 0)
            {
                App.ReceiveChangesExternalEvent.Raise();

            }
        }


        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.SendChangesList.Items.Count >= 0)
            {
                App.ExportChangesExternalEvent.Raise();
            }
        }
    }
}
