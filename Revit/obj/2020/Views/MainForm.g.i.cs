﻿#pragma checksum "..\..\..\Views\MainForm.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "01E80A02D7FAC0BE75A3266C148DB4E9C76D0C325B5FF3497D0F0A61F690C727"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace BIMSocket {
    
    
    /// <summary>
    /// MainForm
    /// </summary>
    public partial class MainForm : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 4 "..\..\..\Views\MainForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal BIMSocket.MainForm MainWindow;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\Views\MainForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button TtitleButton;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\Views\MainForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CloseButton;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\Views\MainForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Send_Button;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\Views\MainForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox SendChangesList;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\Views\MainForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox ReceiveChangesList;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\Views\MainForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Receive_Button;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/BIMSocket;component/views/mainform.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\MainForm.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.MainWindow = ((BIMSocket.MainForm)(target));
            return;
            case 2:
            
            #line 15 "..\..\..\Views\MainForm.xaml"
            ((System.Windows.Controls.Border)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Border_MouseDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.TtitleButton = ((System.Windows.Controls.Button)(target));
            return;
            case 4:
            this.CloseButton = ((System.Windows.Controls.Button)(target));
            return;
            case 5:
            this.Send_Button = ((System.Windows.Controls.Button)(target));
            
            #line 39 "..\..\..\Views\MainForm.xaml"
            this.Send_Button.Click += new System.Windows.RoutedEventHandler(this.Send_Button_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.SendChangesList = ((System.Windows.Controls.ListBox)(target));
            return;
            case 7:
            this.ReceiveChangesList = ((System.Windows.Controls.ListBox)(target));
            return;
            case 8:
            this.Receive_Button = ((System.Windows.Controls.Button)(target));
            
            #line 55 "..\..\..\Views\MainForm.xaml"
            this.Receive_Button.Click += new System.Windows.RoutedEventHandler(this.Receive_Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

