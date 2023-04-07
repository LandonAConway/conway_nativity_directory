using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using NLua;

namespace Conway_Nativity_Directory.Lua_Sandbox
{
    public class StackPanelElement : ContainerElement
    {
        public StackPanelElement()
        {
            Construct();
        }

        public StackPanelElement(object width, object height)
        {
            Construct();
            SetWidth(width);
            SetHeight(height);
        }

        protected override void Construct()
        {
            stackPanel = new StackPanel();
            base.Construct();
        }

        internal override UIElement UIElement => stackPanel;
        private StackPanel stackPanel;

        public string GetOrientation() => stackPanel.Orientation.ToString();
        public void SetOrientation(string value)
        {
            stackPanel.Orientation = (Orientation)Enum.Parse(typeof(Orientation), value);
            RunCallback(OnOrientationChanged, this, value);
        }

        public string GetFlowDirection() => stackPanel.FlowDirection.ToString();
        public void SetFlowDirection(string value)
        {
            stackPanel.FlowDirection = (FlowDirection)Enum.Parse(typeof(FlowDirection), value);
            RunCallback(OnFlowDirectionChanged, this, value);
        }

        //elements
        protected override void OnAddElement(string name, CndFormElement added, string result)
        {
            if (added != null)
                stackPanel.Children.Add(added.UIElement);
        }

        protected override void OnRemoveElement(string name, CndFormElement removed, string result)
        {
            if (removed != null)
                stackPanel.Children.Remove(removed.UIElement);
        }

        //callbacks
        public LuaFunction OnOrientationChanged;
        public LuaFunction OnFlowDirectionChanged;
    }
}
