using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using NLua;
using static System.Net.Mime.MediaTypeNames;

namespace Conway_Nativity_Directory.Lua_Sandbox
{
    public class TextBoxElement : CndFormElement
    {
        public TextBoxElement()
        {
            Construct();
        }

        public TextBoxElement(object width, object height) : base(width, height) { }

        public TextBoxElement(string text)
        {
            Construct();
            SetText(text);
        }

        public TextBoxElement(string text, double width, double height) : base(width, height)
        {
            Construct();
            SetText(text);
        }

        protected override void Construct()
        {
            textBox = new TextBox();
            textBox.TextChanged += new TextChangedEventHandler((object sender, TextChangedEventArgs e) =>
            {
                RunCallback(OnTextChanged, this, GetText());
            });
            textBox.GotKeyboardFocus += new KeyboardFocusChangedEventHandler((object sender, KeyboardFocusChangedEventArgs e) =>
            {
                RunCallback(OnGotFocus, this);
            });
            textBox.LostKeyboardFocus += new KeyboardFocusChangedEventHandler((object sender, KeyboardFocusChangedEventArgs e) =>
            {
                RunCallback(OnLostFocus, this);
            });
            base.Construct();
        }

        internal override UIElement UIElement => textBox;
        private TextBox textBox;

        public string GetText() => textBox.Text;
        public void SetText(string text) => textBox.Text = text;

        public void ForceFocus()
        {
            App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new Action(delegate () {
                    textBox.Focus();
                    textBox.CaretIndex = textBox.Text.Count();
                    Keyboard.Focus(textBox);
                }));
        }


        //Callbacks
        public LuaFunction OnTextChanged { get; set; }
        public LuaFunction OnGotFocus { get; set; }
        public LuaFunction OnLostFocus { get; set; }
    }
}
