using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for CustomTextbox.xaml
    /// </summary>
    public partial class CustomTextbox : TextBox
    {
        public CustomTextbox()
        {
            InitializeComponent();
            LoadTimer();
        }


        #region Masking

        public static readonly DependencyProperty RegexMaskProperty =
            DependencyProperty.Register("RegexMask", typeof(string), typeof(CustomTextbox), new PropertyMetadata(String.Empty));

        public string RegexMask
        {
            get { return (string)GetValue(RegexMaskProperty); }
            set { SetValue(RegexMaskProperty, value); }
        }

        private Regex _regex
        {
            get { return new Regex(RegexMask); }
        }

        private bool IsTextAllowed(string text)
        {
            if (!String.IsNullOrEmpty(RegexMask))
            {
                if (NumericOnly)
                    return (!_regex.IsMatch(text) && IsNumeric(Text.Insert(CaretIndex, text)));
                else
                    return !_regex.IsMatch(text);
            }

            else
            {
                if (NumericOnly)
                    return IsNumeric(Text.Insert(CaretIndex, text));
                else
                    return true;
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));

                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }

            else
            {
                e.CancelCommand();
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space || e.Key == Key.Tab)
            {
                if (!IsTextAllowed(" "))
                {
                    e.Handled = true;
                }
            }

            base.OnPreviewKeyDown(e);
        }

        #endregion


        #region Numeric

        public static readonly DependencyProperty NumericOnlyProperty =
            DependencyProperty.Register("NumericOnly", typeof(bool), typeof(CustomRichTextBox),
                new PropertyMetadata(false));

        public bool NumericOnly
        {
            get { return (bool)GetValue(NumericOnlyProperty); }
            set { SetValue(NumericOnlyProperty, value); }
        }

        private bool IsNumeric(string text)
        {
            System.ComponentModel.TypeConverter _numeric = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Double));
            return _numeric.IsValid(text) && !text.Contains(" ");
        }

        #endregion


        #region Keyboard Focus

        private static readonly DependencyPropertyKey IsKeyboardFocusedPropertyKey =
            DependencyProperty.RegisterReadOnly("IsKeyboardFocused", typeof(bool), typeof(CustomTextbox),
                new PropertyMetadata(false));

        public new static readonly DependencyProperty IsKeyboardFocusedProperty =
            IsKeyboardFocusedPropertyKey.DependencyProperty;

        public new bool IsKeyboardFocused
        {
            get { return (bool)GetValue(IsKeyboardFocusedProperty); }
            protected set { SetValue(IsKeyboardFocusedPropertyKey, value); }
        }

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            IsKeyboardFocused = true;
        }

        private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            IsKeyboardFocused = false;
        }

        #endregion


        #region ParentContainer

        public static readonly DependencyProperty ParentContainerProperty =
            DependencyProperty.Register("ParentContainer", typeof(object), typeof(CustomTextbox));

        public object ParentContainer
        {
            get { return (object)GetValue(ParentContainerProperty); }
            set { SetValue(ParentContainerProperty, value); }
        }

        #endregion


        #region UserFinishedTyping


        public event UserStoppedTypingEventHandler UserStoppedTyping;

        DispatcherTimer typingTimer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(0.7),
        };

        private void LoadTimer()
        {
            typingTimer.Tick += TypingTimer_Tick;
        }

        private void TypingTimer_Tick(object sender, EventArgs e)
        {
            typingTimer.Stop();
            UserStoppedTyping?.Invoke(this, new UserStoppedTypingEventArgs(lastText, Text));
            lastText = Text;
        }

        private string lastText = String.Empty;
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            lastText = Text;
            base.OnGotFocus(e);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            typingTimer.Stop();
            if (IsKeyboardFocused)
                typingTimer.Start();

            base.OnTextChanged(e);
        }


        #endregion
    }
}
