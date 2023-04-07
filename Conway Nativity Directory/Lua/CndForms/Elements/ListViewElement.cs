using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using Microsoft.JScript;
using NLua;

namespace Conway_Nativity_Directory.Lua_Sandbox
{
    public class ListViewElement : ContainerElement
    {
        public ListViewElement()
        {
            Construct();
        }

        public ListViewElement(object width, object height)
        {
            Construct();
            SetWidth(width);
            SetHeight(height);
        }

        protected override void Construct()
        {
            listView = new ListView();
            base.Construct();
        }

        internal override UIElement UIElement => listView;
        private ListView listView;

        //elements
        protected override void OnAddElement(string name, CndFormElement added, string result)
        {
            if (added != null)
                listView.Items.Add(added.UIElement);
        }

        protected override void OnRemoveElement(string name, CndFormElement removed, string result)
        {
            if (removed != null)
                listView.Items.Remove(removed.UIElement);
        }
    }
}
