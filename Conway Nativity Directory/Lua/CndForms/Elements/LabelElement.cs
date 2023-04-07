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
    public class LabelElement : CndFormElement
    {
        public LabelElement()
        {
            Construct();
        }

        public LabelElement(string text)
        {
            Construct();
            SetText(text);
        }

        protected override void Construct()
        {
            textBlock = new TextBlock();
            base.Construct();
        }

        internal override UIElement UIElement => textBlock;
        private TextBlock textBlock;

        public string GetText() => textBlock.Text;
        public void SetText(string text)
        {
            textBlock.Text = text;
            RunCallback(OnTextChanged, text);
        }


        //Callbacks
        public LuaFunction OnTextChanged { get; set; }
    }
}
