﻿#pragma checksum "..\..\..\com.rta.vsd.hh.ui\PrintPreview.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9F95D7C29175B0E4C92BFF3ABF0DF04E33DCFBFC"
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
using System.Windows.Forms.Integration;
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
using VSDApp.Properties;


namespace VSDApp.com.rta.vsd.hh.ui {
    
    
    /// <summary>
    /// PrintPreview
    /// </summary>
    public partial class PrintPreview : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 21 "..\..\..\com.rta.vsd.hh.ui\PrintPreview.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.FlowDocumentReader DocumentReader;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\com.rta.vsd.hh.ui\PrintPreview.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Documents.BlockUIContainer ImageContainer;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\com.rta.vsd.hh.ui\PrintPreview.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image imag;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\com.rta.vsd.hh.ui\PrintPreview.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel btnStackePanel;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\com.rta.vsd.hh.ui\PrintPreview.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnback;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\com.rta.vsd.hh.ui\PrintPreview.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnPrint;
        
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
            System.Uri resourceLocater = new System.Uri("/VSDApp;component/com.rta.vsd.hh.ui/printpreview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\com.rta.vsd.hh.ui\PrintPreview.xaml"
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
            this.DocumentReader = ((System.Windows.Controls.FlowDocumentReader)(target));
            return;
            case 2:
            this.ImageContainer = ((System.Windows.Documents.BlockUIContainer)(target));
            return;
            case 3:
            this.imag = ((System.Windows.Controls.Image)(target));
            return;
            case 4:
            this.btnStackePanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 5:
            this.btnback = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\..\com.rta.vsd.hh.ui\PrintPreview.xaml"
            this.btnback.Click += new System.Windows.RoutedEventHandler(this.btnback_Click_1);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnPrint = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\com.rta.vsd.hh.ui\PrintPreview.xaml"
            this.btnPrint.Click += new System.Windows.RoutedEventHandler(this.btnPrint_Click_1);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
