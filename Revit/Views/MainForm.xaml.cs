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

        internal static ObservableCollection<ElementId> changedElements;




        /// <summary>
        /// Check if the suffix is a number or not
        /// </summary>
        /// <param name="cmddata_p"></param>
        public MainForm(ExternalCommandData cmddata_p)
        {
            this.DataContext = this;
            this.p_commanddata = cmddata_p;
            changedElements = new ObservableCollection<ElementId>();
            InitializeComponent();
            bool connected = ConnectToDB();
 
            this.ChangesList.ItemsSource = changedElements;
        }

        private bool ConnectToDB()
        {
            
            return FireBaseConnection.Connect();
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
            App.exEvent.Raise();



        }

        public static void AddItem(ElementId elementId)
        {
            changedElements.Add(elementId);
        }

        public static void RemoveItem(ElementId elementId)
        {
            changedElements.Remove(elementId);
        }
    }
}
