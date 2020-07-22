using BIMSocket.Models;
using BIMSocket.Utils;
using Rhino;
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
    /// 




    public partial class MainForm : Window
    {
        private RhinoDoc doc;
        private string projectName;

        /// <summary>
        /// Check if the suffix is a number or not
        /// </summary>

        private static object _changedElementsLock = new object();

        internal static ObservableCollection<string> _changedElements;

        private static object _receviedElementslock = new object();

        internal static ObservableCollection<string> _receivedElements;


        public MainForm(RhinoDoc doc)
        {

            InitializeComponent();

            _changedElements = new ObservableCollection<string>();
            BindingOperations.EnableCollectionSynchronization(_changedElements, _changedElementsLock);
            _receivedElements = new ObservableCollection<string>();
            BindingOperations.EnableCollectionSynchronization(_receivedElements, _receviedElementslock);

            this.SendChangesList.ItemsSource = _changedElements;
            this.ReceiveChangesList.ItemsSource = _receivedElements;

            cleanVariables();
            this.doc = doc;
            projectName = doc.Name.Replace(".3dm", "");
            var connected = ConnectToDB();
            if (connected)
            {
                SetRealTimeConnection();
            }


        }

        private void SetRealTimeConnection()
        {
            FireBaseConnection.ReceiveChangesFromDB();
        }

        private void cleanVariables()
        {
            _changedElements.Clear();

            RhinoManagement.changedElements = new List<string>();

            _receivedElements.Clear();

            RhinoManagement.geometryChanges = new Dictionary<string, Child>();

        }

        private void ClosingWindow(object sender, EventArgs e)
        {
            cleanVariables();
        }


        public static void AddChangedItem(string guid)
        {
            lock (_changedElementsLock)
            {
                if (!_changedElements.Contains(guid))
                {
                    _changedElements.Add(guid);
                }
            }

        }

        public static void ClearChangedItems()
        {
            lock (_changedElementsLock)
            {
                _changedElements.Clear();
            }

        }

        public static void AddReceivedItem(string guid)
        {
            lock (_receviedElementslock)
            {
                if (!_receivedElements.Contains(guid))
                {
                    _receivedElements.Add(guid);
                }
                
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

           return FireBaseConnection.Connect("models", projectName);
        }

    
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
            string ro =   RhinoManagement.ConvertModelToString();
            if (ro != null)
            {
                FireBaseConnection.SendModelToDB(ro, "models", projectName);
            }
            
        }



        private void Receive_Button_Click(object sender, RoutedEventArgs e)
        {
            RhinoManagement.ApplyChanges();
            MainForm.ClearReceivedItems();
        }


        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {

            string ro = RhinoManagement.ConvertChangesToString();
            if (ro != null)
            {

                FireBaseConnection.SendChangesToDB(ro, null);
            }

            
            MainForm.ClearChangedItems();
        }
    }
}
