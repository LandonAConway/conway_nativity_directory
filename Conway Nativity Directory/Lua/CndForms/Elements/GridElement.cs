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
    public class GridElement : ContainerElement
    {
        public GridElement()
        {
            Construct();
        }

        public GridElement(object width, object height)
        {
            Construct();
            SetWidth(width);
            SetHeight(height);
        }

        protected override void Construct()
        {
            grid = new Grid();
            base.Construct();
        }

        internal override UIElement UIElement => grid;
        private Grid grid;

        public void AddRowDefinition(object value)
        {
            string height = value.ToString();
            RowDefinition rowDefinition = new RowDefinition();
            GridLengthConverter converter = new GridLengthConverter();
            rowDefinition.Height = (GridLength)converter.ConvertFromString(height);
            grid.RowDefinitions.Add(rowDefinition);
        }

        public void AddColumnDefinition(object value)
        {
            string width = value.ToString();
            ColumnDefinition columnDefinition = new ColumnDefinition();
            GridLengthConverter converter = new GridLengthConverter();
            columnDefinition.Width = (GridLength)converter.ConvertFromString(width);
            grid.ColumnDefinitions.Add(columnDefinition);
        }

        public void ClearColumnDefinitions() => grid.ColumnDefinitions.Clear();
        public void ClearRowDefinitions() => grid.RowDefinitions.Clear();

        //elements
        protected override void OnAddElement(string name, CndFormElement added, string result)
        {
            if (added != null)
                grid.Children.Add(added.UIElement);
        }

        protected override void OnRemoveElement(string name, CndFormElement removed, string result)
        {
            if (removed != null)
                grid.Children.Remove(removed.UIElement);
        }
    }
}
