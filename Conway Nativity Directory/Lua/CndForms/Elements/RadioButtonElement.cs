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
    public class RadioButtonElement : CndFormElement
    {
        public RadioButtonElement()
        {
            Construct();
        }

        public RadioButtonElement(object width, object height)
        {
            Construct();
            SetWidth(width);
            SetHeight(height);
        }

        public RadioButtonElement(string text)
        {
            Construct();
            SetText(text);
        }

        public RadioButtonElement(string text, double width, double height)
        {
            Construct();
            SetText(text);
            SetWidth(width);
            SetHeight(height);
        }

        protected override void Construct()
        {
            radioButton = new RadioButton();

            radioButton.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                RunCallback(OnClick, this);
            });
            radioButton.Checked += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                RunCallback(OnIsCheckedChanged, this, true);
            });
            radioButton.Unchecked += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                RunCallback(OnIsCheckedChanged, this, false);
            });
            base.Construct();
        }

        internal override UIElement UIElement => radioButton;
        private RadioButton radioButton;

        public string GetText() => radioButton.Content.ToString();
        public void SetText(string text)
        {
            radioButton.Content = text;
            RunCallback(OnTextChanged, text);
        }

        public bool GetIsChecked() => radioButton.IsChecked.Value;
        public void SetIsChecked(bool value) => radioButton.IsChecked = value;


        //Callbacks
        public LuaFunction OnTextChanged { get; set; }
        public LuaFunction OnClick { get; set; }
        public LuaFunction OnIsCheckedChanged { get; set; }
    }
}
