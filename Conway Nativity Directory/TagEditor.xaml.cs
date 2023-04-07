using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;
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
using Microsoft.Win32;

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for TagEditor.xaml
    /// </summary>
    public partial class TagEditor : UserControl
    {


        #region Constructor
        
        public TagEditor()
        {
            InitializeComponent();
            SetBindings();
        }

        #endregion


        #region Event Handlers


        public event TagsChangedEventHandler UserChangedTags;


        #endregion


        #region Dependency Properties

        public static readonly DependencyProperty TagsSourceProperty =
            DependencyProperty.Register("TagsSource", typeof(ObservableCollection<string>), typeof(TagEditor));

        public ObservableCollection<string> TagsSource
        {
            get { return (ObservableCollection<string>)GetValue(TagsSourceProperty); }
            set { SetValue(TagsSourceProperty, value); }
        }


        public static readonly DependencyProperty TagsBrushProperty =
            DependencyProperty.Register("TagsBrush", typeof(Brush), typeof(TagEditor), new PropertyMetadata(SystemColors.InactiveCaptionBrush));

        public Brush TagsBrush
        {
            get { return (Brush)GetValue(TagsBrushProperty); }
            set { SetValue(TagsBrushProperty, value); }
        }


        public static readonly DependencyProperty MovingTagsBorderBrushProperty =
            DependencyProperty.Register("MovingTagsBorderBrush", typeof(Brush), typeof(TagEditor), new PropertyMetadata(SystemColors.HotTrackBrush));

        public Brush MovingTagsBorderBrush
        {
            get { return (Brush)GetValue(MovingTagsBorderBrushProperty); }
            set { SetValue(MovingTagsBorderBrushProperty, value); }
        }


        public static readonly DependencyProperty TagsMouseOverBrushProperty =
            DependencyProperty.Register("TagsMouseOverBrush", typeof(Brush), typeof(TagEditor), new PropertyMetadata(SystemColors.HighlightBrush));

        public Brush TagsMouseOverBrush
        {
            get { return (Brush)GetValue(TagsMouseOverBrushProperty); }
            set { SetValue(TagsMouseOverBrushProperty, value); }
        }


        public static readonly DependencyProperty EditingTagsBrushProperty =
            DependencyProperty.Register("EditingTagsBrush", typeof(Brush), typeof(TagEditor), new PropertyMetadata(SystemColors.ActiveCaptionBrush));

        public Brush EditingTagsBrush
        {
            get { return (Brush)GetValue(EditingTagsBrushProperty); }
            set { SetValue(EditingTagsBrushProperty, value); }
        }


        public static readonly DependencyProperty EditingTagsMouseOverBrushProperty =
            DependencyProperty.Register("EditingTagsMouseOverBrush", typeof(Brush), typeof(TagEditor), new PropertyMetadata(SystemColors.HighlightBrush));

        public Brush EditingTagsMouseOverBrush
        {
            get { return (Brush)GetValue(EditingTagsMouseOverBrushProperty); }
            set { SetValue(EditingTagsMouseOverBrushProperty, value); }
        }


        public static readonly DependencyProperty NewTagBrushProperty =
            DependencyProperty.Register("NewTagBrush", typeof(Brush), typeof(TagEditor), new PropertyMetadata(Brushes.Gold));

        public Brush NewTagBrush
        {
            get { return (Brush)GetValue(NewTagBrushProperty); }
            set { SetValue(NewTagBrushProperty, value); }
        }


        public new static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(Brush), typeof(TagEditor), new PropertyMetadata(SystemColors.InactiveCaptionTextBrush));

        public new Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }


        public static readonly DependencyProperty EditingForegroundProperty =
            DependencyProperty.Register("EditingForeground", typeof(Brush), typeof(TagEditor), new PropertyMetadata(SystemColors.ActiveCaptionTextBrush));

        public Brush EditingForeground
        {
            get { return (Brush)GetValue(EditingForegroundProperty); }
            set { SetValue(EditingForegroundProperty, value); }
        }


        public static readonly DependencyProperty EditingMouseOverForegroundProperty =
            DependencyProperty.Register("EditingMouseOverForeground", typeof(Brush), typeof(TagEditor), new PropertyMetadata(SystemColors.ActiveCaptionTextBrush));

        public Brush EditingMouseOverForeground
        {
            get { return (Brush)GetValue(EditingMouseOverForegroundProperty); }
            set { SetValue(EditingMouseOverForegroundProperty, value); }
        }


        public static readonly DependencyProperty NewTagForegroundProperty =
            DependencyProperty.Register("NewTagForeground", typeof(Brush), typeof(TagEditor), new PropertyMetadata(SystemColors.ControlLightLightBrush));

        public Brush NewTagForeground
        {
            get { return (Brush)GetValue(NewTagForegroundProperty); }
            set { SetValue(NewTagForegroundProperty, value); }
        }


        public static readonly DependencyProperty ButtonsBrushProperty =
            DependencyProperty.Register("ButtonsBrush", typeof(Brush), typeof(TagEditor), new PropertyMetadata(SystemColors.InactiveCaptionBrush));

        public Brush ButtonsBrush
        {
            get { return (Brush)GetValue(ButtonsBrushProperty); }
            set { SetValue(ButtonsBrushProperty, value); }
        }


        public static readonly DependencyProperty NewTagButtonsBrushProperty =
            DependencyProperty.Register("NewTagButtonsBrush", typeof(Brush), typeof(TagEditor), new PropertyMetadata(SystemColors.ControlLightLightBrush));

        public Brush NewTagButtonsBrush
        {
            get { return (Brush)GetValue(NewTagButtonsBrushProperty); }
            set { SetValue(NewTagButtonsBrushProperty, value); }
        }


        public static readonly DependencyProperty EditingButtonsBrushProperty =
            DependencyProperty.Register("EditingButtonsBrush", typeof(Brush), typeof(TagEditor), new PropertyMetadata(SystemColors.ActiveCaptionBrush));

        public Brush EditingButtonsBrush
        {
            get { return (Brush)GetValue(EditingButtonsBrushProperty); }
            set { SetValue(EditingButtonsBrushProperty, value); }
        }


        public static readonly DependencyProperty TagButtonsBrushProperty =
            DependencyProperty.Register("TagButtonsBrush", typeof(Brush), typeof(TagEditor), new PropertyMetadata(SystemColors.ControlLightLightBrush));

        public Brush TagButtonsBrush
        {
            get { return (Brush)GetValue(TagButtonsBrushProperty); }
            set { SetValue(TagButtonsBrushProperty, value); }
        }


        public static readonly DependencyProperty ButtonsMouseOverBrushProperty =
            DependencyProperty.Register("ButtonsMouseOverBrush", typeof(Brush), typeof(TagEditor), new PropertyMetadata(SystemColors.InactiveCaptionTextBrush));

        public Brush ButtonsMouseOverBrush
        {
            get { return (Brush)GetValue(ButtonsMouseOverBrushProperty); }
            set { SetValue(ButtonsMouseOverBrushProperty, value); }
        }


        public static readonly DependencyProperty NewTagButtonsMouseOverBrushProperty =
            DependencyProperty.Register("NewTagButtonsMouseOverBrush", typeof(Brush), typeof(TagEditor), new PropertyMetadata(SystemColors.ActiveCaptionTextBrush));

        public Brush NewTagButtonsMouseOverBrush
        {
            get { return (Brush)GetValue(NewTagButtonsMouseOverBrushProperty); }
            set { SetValue(NewTagButtonsMouseOverBrushProperty, value); }
        }


        public static readonly DependencyProperty EditingButtonsMouseOverBrushProperty =
            DependencyProperty.Register("EditingButtonsMouseOverBrush", typeof(Brush), typeof(TagEditor), new PropertyMetadata(SystemColors.ActiveCaptionTextBrush));

        public Brush EditingButtonsMouseOverBrush
        {
            get { return (Brush)GetValue(EditingButtonsMouseOverBrushProperty); }
            set { SetValue(EditingButtonsMouseOverBrushProperty, value); }
        }


        public static readonly DependencyProperty TagButtonsMouseOverBrushProperty =
            DependencyProperty.Register("TagButtonsMouseOverBrush", typeof(Brush), typeof(TagEditor), new PropertyMetadata(SystemColors.ActiveCaptionTextBrush));

        public Brush TagButtonsMouseOverBrush
        {
            get { return (Brush)GetValue(TagButtonsMouseOverBrushProperty); }
            set { SetValue(TagButtonsMouseOverBrushProperty, value); }
        }


        #region Read-only


        private static readonly DependencyPropertyKey EditingPropertyKey
        = DependencyProperty.RegisterReadOnly(
            nameof(Editing), typeof(bool), typeof(TagEditor),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));

        public static readonly DependencyProperty EditingProperty
            = EditingPropertyKey.DependencyProperty;

        public bool Editing
        {
            get { return (bool)GetValue(EditingProperty); }
            protected set { SetValue(EditingPropertyKey, value); }
        }


        private static readonly DependencyPropertyKey AddingPropertyKey
        = DependencyProperty.RegisterReadOnly(
            nameof(Adding), typeof(bool), typeof(TagEditor),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));

        public static readonly DependencyProperty AddingProperty
            = AddingPropertyKey.DependencyProperty;

        public bool Adding
        {
            get { return (bool)GetValue(AddingProperty); }
            protected set { SetValue(AddingPropertyKey, value); }
        }


        private static readonly DependencyPropertyKey MovingPropertyKey
        = DependencyProperty.RegisterReadOnly(
            nameof(Moving), typeof(bool), typeof(TagEditor),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));

        public static readonly DependencyProperty MovingProperty
            = MovingPropertyKey.DependencyProperty;

        public bool Moving
        {
            get { return (bool)GetValue(MovingProperty); }
            protected set { SetValue(MovingPropertyKey, value); }
        }


        #endregion

        #endregion


        #region Public Properties

        #endregion


        #region Private Properties


        #region For get-only public properties

        private ObservableCollection<string> oldTags;

        #endregion


        #endregion


        #region Public Methods

        public void StartEdit()
        {
            if (!Editing)
            {
                Editing = true;

                RefreshTags();
                BackupTags();
                getKeyboardFocus();
            }
        }

        public void CancelEdit()
        {
            if (Editing)
            {
                Adding = false;
                Editing = false;

                RestoreTags();
            }
        }

        public void EndEdit()
        {
            if (Editing)
            {
                Adding = false;
                Editing = false;
            }
        }

        public void StartNewTag()
        {
            if (Editing)
            {
                if (!Adding)
                {
                    Adding = true;
                    focusNewTagTextBox();
                }
            }
        }

        public void CancelNewTag()
        {
            if (Editing)
            {
                if (Adding)
                {
                    Adding = false;
                    newTagTextBox.Text = String.Empty;
                    getKeyboardFocus();
                }
            }
        }

        public void EndNewTag()
        {
            if (Editing)
            {
                if (Adding)
                {
                    Adding = false;
                    TagsSource.Insert(0, newTagTextBox.Text);
                    newTagTextBox.Text = String.Empty;
                    getKeyboardFocus();
                }
            }
        }

        public void RefreshTags()
        {
            BackupTags();
            RestoreTags();
        }

        #endregion


        #region Private Methods

        private void BackupTags()
        {
            oldTags = new ObservableCollection<string>(TagsSource);
        }

        private void RestoreTags()
        {
            TagsSource.Clear();
            foreach (string tag in oldTags)
            {
                TagsSource.Add(tag);
            }
        }

        private void focusNewTagTextBox()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
                        new Action(delegate ()
                        {
                            newTagTextBox.Focus();
                            newTagTextBox.CaretIndex = newTagTextBox.Text.Count();
                            Keyboard.Focus(newTagTextBox);
                        }));
        }

        private void getKeyboardFocus()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
                        new Action(delegate ()
                        {
                            this.Focus();
                            Keyboard.Focus(this);
                        }));
        }

        private void SetBindings()
        {
            InterfaceMainWindowPreference preference = (InterfaceMainWindowPreference)App.Preferences.GetPreference(@"Interface\Main Window");

            App.SetBinding("TButtonsBrush", preference, this, ButtonsBrushProperty,
            BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TButtonsMouseOverBrush", preference, this, ButtonsMouseOverBrushProperty,
            BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TEditingButtonsBrush", preference, this, EditingButtonsBrushProperty,
            BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TEditingButtonsMouseOverBrush", preference, this, EditingButtonsMouseOverBrushProperty,
            BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TEditingTagsBrush", preference, this, EditingTagsBrushProperty,
            BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TEditingTagsMouseOverBrush", preference, this, EditingTagsMouseOverBrushProperty,
            BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TEditingForeground", preference, this, EditingForegroundProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TEditingMouseOverForeground", preference, this, EditingMouseOverForegroundProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TMovingTagsBorderBrush", preference, this, MovingTagsBorderBrushProperty,
            BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TNewTagBrush", preference, this, NewTagBrushProperty,
            BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TNewTagButtonsBrush", preference, this, NewTagButtonsBrushProperty,
            BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TNewTagButtonsMouseOverBrush", preference, this, NewTagButtonsMouseOverBrushProperty,
            BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TNewTagForeground", preference, this, NewTagForegroundProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TTagButtonsBrush", preference, this, TagButtonsBrushProperty,
            BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TTagButtonsMouseOverBrush", preference, this, TagButtonsMouseOverBrushProperty,
            BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TTagsBrush", preference, this, TagsBrushProperty,
            BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TTagsMouseOverBrush", preference, this, TagsMouseOverBrushProperty,
            BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TForeground", preference, this, ForegroundProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);
        }

        #endregion


        #region Events

        private void Tag_RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            TagsSource.Remove((sender as Tag).Text);
        }

        private void ConfirmNewTagButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            EndNewTag();
        }

        private void CancelNewTagButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CancelNewTag();
        }

        private void Edit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StartEdit();
        }

        private void Confirm_MouseDown(object sender, MouseButtonEventArgs e)
        {
            EndEdit();
            UserChangedTags?.Invoke(this, new TagsChangedEventArgs(oldTags.ToArray(), TagsSource.ToArray()));
        }

        private void Cancel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CancelEdit();
        }

        private void AddTag_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StartNewTag();
        }

        private void CommandBinding_Confirm(object sender, ExecutedRoutedEventArgs e)
        {
            if (Editing && Adding)
                EndNewTag();
            else if (Editing)
                EndEdit();
        }

        private void CommandBinding_Cancel(object sender, ExecutedRoutedEventArgs e)
        {
            if (Editing && Adding)
                CancelNewTag();
            else if (Editing)
                CancelEdit();
        }



        #endregion


        #region Moveable Tags

        private Tag firstTag;
        private Tag secondTag;
        private void Tag_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                if (firstTag == null)
                {
                    firstTag = sender as Tag;
                }
                
                secondTag = sender as Tag;
            }

            else
            {
                firstTag = null;
                secondTag = null;
            }
        }

        private bool mouseDown = false;
        private void Tags_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Editing)
            {
                mouseDown = true;
                Moving = true;
            }
        }

        private void Tags_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Editing)
            {
                if (firstTag != null)
                {
                    int _firstIndex = TagsSource.IndexOf(firstTag.Text);
                    int _secondIndex = TagsSource.IndexOf(secondTag.Text);

                    TagsSource.Move(_firstIndex, _secondIndex);

                    Moving = false;
                    mouseDown = false;
                }
            }
        }

        private void Tags_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (Editing)
            {
                if (mouseDown)
                {
                    
                }
            }
        }


        #endregion

        private void tags_Loaded(object sender, RoutedEventArgs e)
        {
        }

        internal readonly List<Tag> tagControls = new List<Tag>();
        private void Tag_Loaded(object sender, RoutedEventArgs e)
        {
            ((Tag)sender).Unloaded += TagEditor_Unloaded;
            tagControls.Add((Tag)sender);
        }

        private void TagEditor_Unloaded(object sender, RoutedEventArgs e)
        {
            tagControls.Remove((Tag)sender);
        }
    }

    public static class MouseInterop
    {
        public static void LeftClick()
        {
            var x = (uint)System.Windows.Forms.Cursor.Position.X;
            var y = (uint)System.Windows.Forms.Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
    }

    public delegate void TagsChangedEventHandler(object sender, TagsChangedEventArgs e);

    public class TagsChangedEventArgs : EventArgs
    {
        private string[] oldTags;
        public string[] OldTags { get { return oldTags; } }

        private string[] newTags;
        public string[] NewTags { get { return newTags; } }

        public TagsChangedEventArgs(string[] oldTags, string[] newTags)
        {
            this.oldTags = oldTags;
            this.newTags = newTags;
        }
    }
}