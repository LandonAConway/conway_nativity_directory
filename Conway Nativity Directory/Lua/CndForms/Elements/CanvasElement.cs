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
    public class CanvasElement : ContainerElement
    {
        public CanvasElement()
        {
            Construct();
        }

        public CanvasElement(object width, object height)
        {
            Construct();
            SetWidth(width);
            SetHeight(height);
        }

        protected override void Construct()
        {
            canvas = new Canvas();
            base.Construct();
        }

        internal override UIElement UIElement => canvas;
        private Canvas canvas;

        //elements
        protected override void OnAddElement(string name, CndFormElement added, string result)
        {
            if (added != null)
                canvas.Children.Add(added.UIElement);
        }

        protected override void OnRemoveElement(string name, CndFormElement removed, string result)
        {
            if (removed != null)
                canvas.Children.Remove(removed.UIElement);
        }
    }
}
