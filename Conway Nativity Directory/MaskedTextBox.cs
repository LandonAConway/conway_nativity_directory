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
    public class MaskedTextBox : TextBox
    {
        public MaskedTextBox()
        {
            LoadTimer();
            this.PreviewTextInput += TextBox_PreviewTextInput;
            DataObject.AddPastingHandler(this, new DataObjectPastingEventHandler(TextBoxPasting));
            this.PreviewKeyDown += TextBox_PreviewKeyDown;
            this.GotKeyboardFocus += TextBox_GotKeyboardFocus;
            //this.LostKeyboardFocus += TextBox_LostKeyboardFocus;
        }


        #region Masking

        public static readonly DependencyProperty RegexMaskProperty =
            DependencyProperty.Register("RegexMask", typeof(string), typeof(MaskedTextBox), new PropertyMetadata(String.Empty));

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
            if (e.Key == Key.Space || (e.Key == Key.Tab && AcceptsTab))
            {
                if (!IsTextAllowed(" "))
                {
                    e.Handled = true;
                }
            }

            if (e.Key == Key.Up)
            {
                try
                {
                    var index = App.Project.Nativities.IndexOf(this.Tag);
                    App.MainWindow.nativityListView.SelectedIndex = index - 1;
                    Dispatcher.BeginInvoke(DispatcherPriority.Input,
                        new Action(delegate () {
                            var item = (ListViewItem)App.MainWindow.nativityListView
                                .ItemContainerGenerator
                                .ContainerFromItem(App.MainWindow.nativityListView.SelectedItem);
                            Keyboard.Focus(item);
                        }));
                }

                catch { }
            }

            else if (e.Key == Key.Down)
            {
                try
                {
                    var index = App.Project.Nativities.IndexOf(this.Tag);
                    App.MainWindow.nativityListView.SelectedIndex = index + 1;
                    Dispatcher.BeginInvoke(DispatcherPriority.Input,
                        new Action(delegate () {
                            var item = (ListViewItem)App.MainWindow.nativityListView
                                .ItemContainerGenerator
                                .ContainerFromItem(App.MainWindow.nativityListView.SelectedItem);
                            Keyboard.Focus(item);
                        }));
                }

                catch { }
            }

            base.OnPreviewKeyDown(e);
        }

        #endregion


        #region Numeric

        public static readonly DependencyProperty NumericOnlyProperty =
            DependencyProperty.Register("NumericOnly", typeof(bool), typeof(MaskedTextBox),
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

        //private static readonly DependencyPropertyKey IsKeyboardFocusedPropertyKey =
        //    DependencyProperty.RegisterReadOnly("IsKeyboardFocused", typeof(bool), typeof(MaskedTextBox),
        //        new PropertyMetadata(false));

        //public new static readonly DependencyProperty IsKeyboardFocusedProperty =
        //    IsKeyboardFocusedPropertyKey.DependencyProperty;

        //public new bool IsKeyboardFocused
        //{
        //    get { return (bool)GetValue(IsKeyboardFocusedProperty); }
        //    protected set { SetValue(IsKeyboardFocusedPropertyKey, value); }
        //}

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //IsKeyboardFocused = true;

            Dispatcher.BeginInvoke(DispatcherPriority.Input,
            new Action(delegate () {
                if (System.Windows.Input.Mouse.LeftButton == MouseButtonState.Released)
                    this.CaretIndex = this.Text.Count();
            }));
        }

        //private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        //{
        //    IsKeyboardFocused = false;
        //}

        #endregion


        #region ParentContainer

        public static readonly DependencyProperty ParentContainerProperty =
            DependencyProperty.Register("ParentContainer", typeof(object), typeof(MaskedTextBox));

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

    public delegate void UserStoppedTypingEventHandler(object sender, UserStoppedTypingEventArgs e);

    public class UserStoppedTypingEventArgs : EventArgs
    {
        string oldText;
        public string OldText { get { return oldText; } }

        string newText;
        public string NewText { get { return newText; } }

        public UserStoppedTypingEventArgs(string oldText, string newText)
        {
            this.oldText = oldText;
            this.newText = newText;
        }
    }

    public class NativityColumnElements : DependencyObject
    {
        public static readonly DependencyProperty IdColumnProperty =
            DependencyProperty.RegisterAttached("IdColumn", typeof(FrameworkElement), typeof(NativityColumnElements));

        public static FrameworkElement GetIdColumn(DependencyObject target)
        {
            return (FrameworkElement)target.GetValue(IdColumnProperty);
        }

        public static void SetIdColumn(DependencyObject target, FrameworkElement value)
        {
            target.SetValue(IdColumnProperty, value);
        }


        public static readonly DependencyProperty TitleColumnProperty =
            DependencyProperty.RegisterAttached("TitleColumn", typeof(FrameworkElement), typeof(NativityColumnElements));

        public static FrameworkElement GetTitleColumn(DependencyObject target)
        {
            return (FrameworkElement)target.GetValue(TitleColumnProperty);
        }

        public static void SetTitleColumn(DependencyObject target, FrameworkElement value)
        {
            target.SetValue(TitleColumnProperty, value);
        }


        public static readonly DependencyProperty OriginColumnProperty =
            DependencyProperty.RegisterAttached("OriginColumn", typeof(FrameworkElement), typeof(NativityColumnElements));

        public static FrameworkElement GetOriginColumn(DependencyObject target)
        {
            return (FrameworkElement)target.GetValue(OriginColumnProperty);
        }

        public static void SetOriginColumn(DependencyObject target, FrameworkElement value)
        {
            target.SetValue(OriginColumnProperty, value);
        }


        public static readonly DependencyProperty AcquiredColumnProperty =
            DependencyProperty.RegisterAttached("AcquiredColumn", typeof(FrameworkElement), typeof(NativityColumnElements));

        public static FrameworkElement GetAcquiredColumn(DependencyObject target)
        {
            return (FrameworkElement)target.GetValue(AcquiredColumnProperty);
        }

        public static void SetAcquiredColumn(DependencyObject target, FrameworkElement value)
        {
            target.SetValue(AcquiredColumnProperty, value);
        }


        public static readonly DependencyProperty FromColumnProperty =
            DependencyProperty.RegisterAttached("FromColumn", typeof(FrameworkElement), typeof(NativityColumnElements));

        public static FrameworkElement GetFromColumn(DependencyObject target)
        {
            return (FrameworkElement)target.GetValue(FromColumnProperty);
        }

        public static void SetFromColumn(DependencyObject target, FrameworkElement value)
        {
            target.SetValue(FromColumnProperty, value);
        }


        public static readonly DependencyProperty CostColumnProperty =
            DependencyProperty.RegisterAttached("CostColumn", typeof(FrameworkElement), typeof(NativityColumnElements));

        public static FrameworkElement GetCostColumn(DependencyObject target)
        {
            return (FrameworkElement)target.GetValue(CostColumnProperty);
        }

        public static void SetCostColumn(DependencyObject target, FrameworkElement value)
        {
            target.SetValue(CostColumnProperty, value);
        }


        public static readonly DependencyProperty LocationColumnProperty =
            DependencyProperty.RegisterAttached("LocationColumn", typeof(FrameworkElement), typeof(NativityColumnElements));

        public static FrameworkElement GetLocationColumn(DependencyObject target)
        {
            return (FrameworkElement)target.GetValue(LocationColumnProperty);
        }

        public static void SetLocationColumn(DependencyObject target, FrameworkElement value)
        {
            target.SetValue(LocationColumnProperty, value);
        }


        public static readonly DependencyProperty TagsColumnProperty =
            DependencyProperty.RegisterAttached("TagsColumn", typeof(FrameworkElement), typeof(NativityColumnElements));

        public static FrameworkElement GetTagsColumn(DependencyObject target)
        {
            return (FrameworkElement)target.GetValue(TagsColumnProperty);
        }

        public static void SetTagsColumn(DependencyObject target, FrameworkElement value)
        {
            target.SetValue(TagsColumnProperty, value);
        }


        public static readonly DependencyProperty GeographicalOriginsColumnProperty =
            DependencyProperty.RegisterAttached("GeographicalOriginsColumn", typeof(FrameworkElement), typeof(NativityColumnElements));

        public static FrameworkElement GetGeographicalOriginsColumn(DependencyObject target)
        {
            return (FrameworkElement)target.GetValue(GeographicalOriginsColumnProperty);
        }

        public static void SetGeographicalOriginsColumn(DependencyObject target, FrameworkElement value)
        {
            target.SetValue(GeographicalOriginsColumnProperty, value);
        }
    }
}
