﻿#pragma checksum "..\..\..\ManagePluginsWindow\ManagePluginAuthorizationCodesWindow - Copy.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "0422A8F7FE71F65BBA9E44D4B146EE9C1AA0ABDEA016B170A857C06445B7406F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Conway_Nativity_Directory;
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


namespace Conway_Nativity_Directory {
    
    
    /// <summary>
    /// ManagePluginAuthorizationCodesWindow
    /// </summary>
    public partial class ManagePluginAuthorizationCodesWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\..\ManagePluginsWindow\ManagePluginAuthorizationCodesWindow - Copy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView authorizationCodesListView;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\ManagePluginsWindow\ManagePluginAuthorizationCodesWindow - Copy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button closeBtn;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\ManagePluginsWindow\ManagePluginAuthorizationCodesWindow - Copy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button addBtn;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\ManagePluginsWindow\ManagePluginAuthorizationCodesWindow - Copy.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button removeBtn;
        
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
            System.Uri resourceLocater = new System.Uri("/Conway Nativity Directory;component/managepluginswindow/managepluginauthorizatio" +
                    "ncodeswindow%20-%20copy.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ManagePluginsWindow\ManagePluginAuthorizationCodesWindow - Copy.xaml"
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
            this.authorizationCodesListView = ((System.Windows.Controls.ListView)(target));
            return;
            case 2:
            this.closeBtn = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\..\ManagePluginsWindow\ManagePluginAuthorizationCodesWindow - Copy.xaml"
            this.closeBtn.Click += new System.Windows.RoutedEventHandler(this.closeBtn_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.addBtn = ((System.Windows.Controls.Button)(target));
            
            #line 21 "..\..\..\ManagePluginsWindow\ManagePluginAuthorizationCodesWindow - Copy.xaml"
            this.addBtn.Click += new System.Windows.RoutedEventHandler(this.addBtn_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.removeBtn = ((System.Windows.Controls.Button)(target));
            
            #line 22 "..\..\..\ManagePluginsWindow\ManagePluginAuthorizationCodesWindow - Copy.xaml"
            this.removeBtn.Click += new System.Windows.RoutedEventHandler(this.removeBtn_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
