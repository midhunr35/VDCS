﻿#pragma checksum "..\..\..\com.rta.vsd.hh.ui\ucSearchOptionScreen.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "262270131A42AB4F5D2DD48128E0269516D94DED"
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
    /// ucSearchOptionScreen
    /// </summary>
    public partial class ucSearchOptionScreen : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 43 "..\..\..\com.rta.vsd.hh.ui\ucSearchOptionScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rdoBtnVehicleProfile;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\com.rta.vsd.hh.ui\ucSearchOptionScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rdoBtnViolationHistory;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\com.rta.vsd.hh.ui\ucSearchOptionScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rdoBtnOperatorProfile;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\com.rta.vsd.hh.ui\ucSearchOptionScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnNext;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\com.rta.vsd.hh.ui\ucSearchOptionScreen.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnback;
        
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
            System.Uri resourceLocater = new System.Uri("/VSDApp;component/com.rta.vsd.hh.ui/ucsearchoptionscreen.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\com.rta.vsd.hh.ui\ucSearchOptionScreen.xaml"
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
            
            #line 8 "..\..\..\com.rta.vsd.hh.ui\ucSearchOptionScreen.xaml"
            ((VSDApp.com.rta.vsd.hh.ui.ucSearchOptionScreen)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded_1);
            
            #line default
            #line hidden
            return;
            case 2:
            this.rdoBtnVehicleProfile = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 3:
            this.rdoBtnViolationHistory = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 4:
            this.rdoBtnOperatorProfile = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 5:
            this.btnNext = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\..\com.rta.vsd.hh.ui\ucSearchOptionScreen.xaml"
            this.btnNext.Click += new System.Windows.RoutedEventHandler(this.btnNext_Click_1);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnback = ((System.Windows.Controls.Button)(target));
            
            #line 48 "..\..\..\com.rta.vsd.hh.ui\ucSearchOptionScreen.xaml"
            this.btnback.Click += new System.Windows.RoutedEventHandler(this.btnback_Click_1);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

