using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.JScript;
using System.Windows.Media.Media3D;
using NLua;

namespace Conway_Nativity_Directory.Lua_Sandbox
{
    public class ContentElement : CndFormElement
    {
        public ContentElement()
        {
            Construct();
        }

        public ContentElement(object width, object height)
        {
            Construct();
            SetWidth(width);
            SetHeight(height);
        }

        public ContentElement(CndFormElement content)
        {
            Construct();
            SetContent(content);
        }

        public ContentElement(CndFormElement content, object width, object height)
        {
            Construct();
            SetContent(content);
            SetWidth(width);
            SetHeight(height);
        }

        protected override void Construct()
        {
            base.Construct();
            contentControl = new ContentControl();
        }

        internal override UIElement UIElement => contentControl;
        private ContentControl contentControl;
        private CndFormElement content;

        public CndFormElement GetContent() => content;
        public void SetContent(CndFormElement value)
        {
            if (value == null)
            {
                contentControl.Content = null;
                content.parent = null;
                content = null;
            }

            else
            {
                contentControl.Content = value.UIElement;
                value.RemoveFromParent();
                value.parent = this;
                content = value;
            }

            RunCallback(OnContentChanged, this, value);
        }

        //callbacks
        public LuaFunction OnContentChanged { get; set; }
    }
}
