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
    public partial class MainForm : Window
    {
        private RhinoDoc doc;
        private string projectName;

        /// <summary>
        /// Check if the suffix is a number or not
        /// </summary>


        public MainForm(RhinoDoc doc)
        {
            this.doc = doc;
            projectName = doc.Name.Replace(".3dm","");
            var connected = ConnectToDB();
            InitializeComponent();
        }

        private void cleanVariables()
        {


        }

        private void ClosingWindow(object sender, EventArgs e)
        {
            cleanVariables();
        }




        private bool ConnectToDB()
        {

           return FireBaseConnection.Connect("models", this.doc.Name);
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

        }


        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
