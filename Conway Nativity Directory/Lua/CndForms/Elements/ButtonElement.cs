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
    public class ButtonElement : CndFormElement
    {
        public ButtonElement()
        {
            Construct();
        }

        public ButtonElement(object width, object height)
        {
            Construct();
            SetWidth(width);
            SetHeight(height);
        }

        public ButtonElement(string text)
        {
            Construct();
            SetText(text);
        }

        public ButtonElement(string text, object width, object height)
        {
            Construct();
            SetText(text);
            SetWidth(width);
            SetHeight(height);
        }

        protected override void Construct()
        {
            button = new Button();

            button.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                RunCallback(OnClick, this);
            });
            base.Construct();
        }

        internal override UIElement UIElement => button;
        private Button button;

        public string GetText() => button.Content.ToString();
        public void SetText(string text)
        {
            button.Content = text;
            RunCallback(OnTextChanged, text);
        }


        //Callbacks
        public LuaFunction OnTextChanged { get; set; }
        public LuaFunction OnClick { get; set; }
    }
}
