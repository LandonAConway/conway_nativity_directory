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
    public class CheckBoxElement : CndFormElement
    {
        public CheckBoxElement()
        {
            Construct();
        }

        public CheckBoxElement(object width, object height)
        {
            Construct();
            SetWidth(width);
            SetHeight(height);
        }

        public CheckBoxElement(string text)
        {
            Construct();
            SetText(text);
        }

        public CheckBoxElement(string text, double width, double height)
        {
            Construct();
            SetText(text);
            SetWidth(width);
            SetHeight(height);
        }

        protected override void Construct()
        {
            checkBox = new CheckBox();

            checkBox.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                RunCallback(OnClick, this);
            });
            checkBox.Checked += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                RunCallback(OnIsCheckedChanged, this, true);
            });
            checkBox.Unchecked += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                RunCallback(OnIsCheckedChanged, this, false);
            });
            base.Construct();
        }

        internal override UIElement UIElement => checkBox;
        private CheckBox checkBox;

        public string GetText() => checkBox.Content.ToString();
        public void SetText(string text)
        {
            checkBox.Content = text;
            RunCallback(OnTextChanged, text);
        }

        public bool GetIsChecked() => checkBox.IsChecked.Value;
        public void SetIsChecked(bool value) => checkBox.IsChecked = value;


        //Callbacks
        public LuaFunction OnTextChanged { get; set; }
        public LuaFunction OnClick { get; set; }
        public LuaFunction OnIsCheckedChanged { get; set; }
    }
}
