using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using CustRes;

namespace Conway_Nativity_Directory
{
    public class InterfaceMainWindowPreference : TreeItem, IPreference, ITreeItem
    {


        #region Constructor


        public InterfaceMainWindowPreference()
        {
            ui = new InterfaceMainWindowPreferenceUI(this);
        }


        #endregion


        #region Dependency Properties


        #region Nativity Information


        public static readonly DependencyProperty NativityInformationFontSizeProperty =
            DependencyProperty.Register("NativityInformationFontSize", typeof(double), typeof(InterfaceMainWindowPreference),
                new FrameworkPropertyMetadata(12.0));

        public double NativityInformationFontSize
        {
            get { return (double)GetValue(NativityInformationFontSizeProperty); }
            set { SetValue(NativityInformationFontSizeProperty, value); }
        }

        public static readonly DependencyProperty NativityInformationFontFamilyProperty =
            DependencyProperty.Register("NativityInformationFontFamily", typeof(FontFamily), typeof(InterfaceMainWindowPreference),
                new PropertyMetadata(new FontFamily("Segoe UI")));

        public FontFamily NativityInformationFontFamily
        {
            get { return (FontFamily)GetValue(NativityInformationFontFamilyProperty); }
            set { SetValue(NativityInformationFontFamilyProperty, value); }
        }


        #endregion


        #region Tags & Geographical Origins


        //A map of all the properties...
        //ButtonsBrush;
        //ButtonsMouseOverBrush;
        //EditingButtonsBrush;
        //EditingButtonsMouseOverBrush;
        //EditingTagsBrush;
        //EditingTagsMouseOverBrush;
        //EditingForground //
        //EditingMouseOverForground //
        //MovingTagsBorderBrush;
        //NewTagBrush;
        //NewTagButtonsBrush;
        //NewTagButtonsMouseOverBrush;
        //NewTagForeground //
        //TagButtonsBrush;
        //TagButtonsMouseOverBrush;
        //TagsBrush;
        //TagsMouseOverBrush;
        //Foreground //


        public static readonly DependencyProperty TButtonsBrushProperty =
            DependencyProperty.Register("TButtonsBrush", typeof(Brush), typeof(InterfaceMainWindowPreference),
                new PropertyMetadata(TagEditor.ButtonsBrushProperty.DefaultMetadata.DefaultValue));

        public Brush TButtonsBrush
        {
            get { return (Brush)GetValue(TButtonsBrushProperty); }
            set { SetValue(TButtonsBrushProperty, value); }
        }


        public static readonly DependencyProperty TButtonsMouseOverBrushProperty =
            DependencyProperty.Register("TButtonsMouseOverBrush", typeof(Brush), typeof(InterfaceMainWindowPreference),
                new PropertyMetadata(TagEditor.ButtonsMouseOverBrushProperty.DefaultMetadata.DefaultValue));

        public Brush TButtonsMouseOverBrush
        {
            get { return (Brush)GetValue(TButtonsMouseOverBrushProperty); }
            set { SetValue(TButtonsMouseOverBrushProperty, value); }
        }


        public static readonly DependencyProperty TEditingButtonsBrushProperty =
            DependencyProperty.Register("TEditingButtonsBrush", typeof(Brush), typeof(InterfaceMainWindowPreference),
                new PropertyMetadata(TagEditor.EditingButtonsBrushProperty.DefaultMetadata.DefaultValue));

        public Brush TEditingButtonsBrush
        {
            get { return (Brush)GetValue(TEditingButtonsBrushProperty); }
            set { SetValue(TEditingButtonsBrushProperty, value); }
        }


        public static readonly DependencyProperty TEditingButtonsMouseOverBrushProperty =
            DependencyProperty.Register("TEditingButtonsMouseOverBrush", typeof(Brush), typeof(InterfaceMainWindowPreference),
                new PropertyMetadata(TagEditor.EditingButtonsMouseOverBrushProperty.DefaultMetadata.DefaultValue));

        public Brush TEditingButtonsMouseOverBrush
        {
            get { return (Brush)GetValue(TEditingButtonsMouseOverBrushProperty); }
            set { SetValue(TEditingButtonsMouseOverBrushProperty, value); }
        }


        public static readonly DependencyProperty TEditingTagsBrushProperty =
            DependencyProperty.Register("TEditingTagsBrush", typeof(Brush), typeof(InterfaceMainWindowPreference),
                new PropertyMetadata(TagEditor.EditingTagsBrushProperty.DefaultMetadata.DefaultValue));

        public Brush TEditingTagsBrush
        {
            get { return (Brush)GetValue(TEditingTagsBrushProperty); }
            set { SetValue(TEditingTagsBrushProperty, value); }
        }


        public static readonly DependencyProperty TEditingTagsMouseOverBrushProperty =
            DependencyProperty.Register("TEditingTagsMouseOverBrush", typeof(Brush), typeof(InterfaceMainWindowPreference),
                new PropertyMetadata(TagEditor.EditingTagsMouseOverBrushProperty.DefaultMetadata.DefaultValue));

        public Brush TEditingTagsMouseOverBrush
        {
            get { return (Brush)GetValue(TEditingTagsMouseOverBrushProperty); }
            set { SetValue(TEditingTagsMouseOverBrushProperty, value); }
        }


        public static readonly DependencyProperty TEditingForegroundProperty =
            DependencyProperty.Register("TEditingForeground", typeof(Brush), typeof(InterfaceMainWindowPreference),
                new PropertyMetadata(TagEditor.EditingForegroundProperty.DefaultMetadata.DefaultValue));

        public Brush TEditingForeground
        {
            get { return (Brush)GetValue(TEditingForegroundProperty); }
            set { SetValue(TEditingForegroundProperty, value); }
        }


        public static readonly DependencyProperty TEditingMouseOverForegroundProperty =
            DependencyProperty.Register("TEditingMouseOverForeground", typeof(Brush), typeof(InterfaceMainWindowPreference),
                new PropertyMetadata(TagEditor.EditingMouseOverForegroundProperty.DefaultMetadata.DefaultValue));

        public Brush TEditingMouseOverForeground
        {
            get { return (Brush)GetValue(TEditingMouseOverForegroundProperty); }
            set { SetValue(TEditingMouseOverForegroundProperty, value); }
        }


        public static readonly DependencyProperty TMovingTagsBorderBrushProperty =
            DependencyProperty.Register("TMovingTagsBorderBrush", typeof(Brush), typeof(InterfaceMainWindowPreference),
                new PropertyMetadata(TagEditor.MovingTagsBorderBrushProperty.DefaultMetadata.DefaultValue));

        public Brush TMovingTagsBorderBrush
        {
            get { return (Brush)GetValue(TMovingTagsBorderBrushProperty); }
            set { SetValue(TMovingTagsBorderBrushProperty, value); }
        }


        public static readonly DependencyProperty TNewTagBrushProperty =
            DependencyProperty.Register("TNewTagBrush", typeof(Brush), typeof(InterfaceMainWindowPreference),
                new PropertyMetadata(TagEditor.NewTagBrushProperty.DefaultMetadata.DefaultValue));

        public Brush TNewTagBrush
        {
            get { return (Brush)GetValue(TNewTagBrushProperty); }
            set { SetValue(TNewTagBrushProperty, value); }
        }


        public static readonly DependencyProperty TNewTagButtonsBrushProperty =
            DependencyProperty.Register("TNewTagButtonsBrush", typeof(Brush), typeof(InterfaceMainWindowPreference),
                new PropertyMetadata(TagEditor.NewTagButtonsBrushProperty.DefaultMetadata.DefaultValue));

        public Brush TNewTagButtonsBrush
        {
            get { return (Brush)GetValue(TNewTagButtonsBrushProperty); }
            set { SetValue(TNewTagButtonsBrushProperty, value); }
        }


        public static readonly DependencyProperty TNewTagButtonsMouseOverBrushProperty =
            DependencyProperty.Register("TNewTagButtonsMouseOverBrush", typeof(Brush), typeof(InterfaceMainWindowPreference),
                new PropertyMetadata(TagEditor.NewTagButtonsMouseOverBrushProperty.DefaultMetadata.DefaultValue));

        public Brush TNewTagButtonsMouseOverBrush
        {
            get { return (Brush)GetValue(TNewTagButtonsMouseOverBrushProperty); }
            set { SetValue(TNewTagButtonsMouseOverBrushProperty, value); }
        }


        public static readonly DependencyProperty TNewTagForegroundProperty =
            DependencyProperty.Register("TNewTagForeground", typeof(Brush), typeof(InterfaceMainWindowPreference),
                new PropertyMetadata(TagEditor.NewTagForegroundProperty.DefaultMetadata.DefaultValue));

        public Brush TNewTagForeground
        {
            get { return (Brush)GetValue(TNewTagForegroundProperty); }
            set { SetValue(TNewTagForegroundProperty, value); }
        }


        public static readonly DependencyProperty TTagButtonsBrushProperty =
            DependencyProperty.Register("TTagButtonsBrush", typeof(Brush), typeof(InterfaceMainWindowPreference),
                new PropertyMetadata(TagEditor.TagButtonsBrushProperty.DefaultMetadata.DefaultValue));

        public Brush TTagButtonsBrush
        {
            get { return (Brush)GetValue(TTagButtonsBrushProperty); }
            set { SetValue(TTagButtonsBrushProperty, value); }
        }


        public static readonly DependencyProperty TTagButtonsMouseOverBrushProperty =
            DependencyProperty.Register("TTagButtonsMouseOverBrush", typeof(Brush), typeof(InterfaceMainWindowPreference),
                new PropertyMetadata(TagEditor.TagButtonsMouseOverBrushProperty.DefaultMetadata.DefaultValue));

        public Brush TTagButtonsMouseOverBrush
        {
            get { return (Brush)GetValue(TTagButtonsMouseOverBrushProperty); }
            set { SetValue(TTagButtonsMouseOverBrushProperty, value); }
        }


        public static readonly DependencyProperty TTagsBrushProperty =
            DependencyProperty.Register("TTagsBrush", typeof(Brush), typeof(InterfaceMainWindowPreference),
                new PropertyMetadata(TagEditor.TagsBrushProperty.DefaultMetadata.DefaultValue));

        public Brush TTagsBrush
        {
            get { return (Brush)GetValue(TTagsBrushProperty); }
            set { SetValue(TTagsBrushProperty, value); }
        }


        public static readonly DependencyProperty TTagsMouseOverBrushProperty =
            DependencyProperty.Register("TTagsMouseOverBrush", typeof(Brush), typeof(InterfaceMainWindowPreference),
                new PropertyMetadata(TagEditor.TagsMouseOverBrushProperty.DefaultMetadata.DefaultValue));

        public Brush TTagsMouseOverBrush
        {
            get { return (Brush)GetValue(TTagsMouseOverBrushProperty); }
            set { SetValue(TTagsMouseOverBrushProperty, value); }
        }


        public static readonly DependencyProperty TForegroundProperty =
            DependencyProperty.Register("TForeground", typeof(Brush), typeof(InterfaceMainWindowPreference),
                new PropertyMetadata(TagEditor.ForegroundProperty.DefaultMetadata.DefaultValue));

        public Brush TForeground
        {
            get { return (Brush)GetValue(TForegroundProperty); }
            set { SetValue(TForegroundProperty, value); }
        }


        #endregion


        #endregion


        #region Public Properties

        /// <summary>
        /// Gets the title of the preference.
        /// </summary>
        public string Title { get { return "Main Window"; } }

        /// <summary>
        /// Indicates whether the preference is shown in a <see cref="PreferencesWindow"/> or not.
        /// </summary>
        public bool Visible { get { return true; } }

        /// <summary>
        /// Indicates wether or not any changes will take effect the next time the application is restarted.
        /// </summary>
        public bool EffectiveImmediately { get { return true; } }

        #endregion


        #region Private Properties

        #region For get-only properties

        private UIElement ui;

        #endregion

        #endregion


        #region Public Methods

        public void Exceptions()
        {
            foreach (IPreference preference in Items)
            {
                preference.Exceptions();
            }
        }

        public void Apply()
        {
            InterfaceMainWindowPreferenceUI _ui = (InterfaceMainWindowPreferenceUI)ui;
            NativityInformationFontSize = _ui.nativityInformationFontSizeNumericUpDown.Value;
            NativityInformationFontFamily = new FontFamily(_ui.nativityInformationFontFamilyComboBox.Text);
            TButtonsBrush = _ui.tButtonsBorder.Background;
            TButtonsMouseOverBrush = _ui.tButtonsMouseOverBorder.Background;
            TEditingButtonsBrush = _ui.tEditingButtonsBorder.Background;
            TEditingButtonsMouseOverBrush = _ui.tEditingButtonsMouseOverBorder.Background;
            TEditingTagsBrush = _ui.tEditingTagsBorder.Background;
            TEditingTagsMouseOverBrush = _ui.tEditingTagsMouseOverBorder.Background;
            TEditingForeground = _ui.tEditingForegroundBorder.Background;
            TEditingMouseOverForeground = _ui.tEditingMouseOverForegroundBorder.Background;
            TMovingTagsBorderBrush = _ui.tMovingTagsBorder.Background;
            TNewTagBrush = _ui.tNewTagBorder.Background;
            TNewTagButtonsBrush = _ui.tNewTagButtonsBorder.Background;
            TNewTagButtonsMouseOverBrush = _ui.tNewTagButtonsMouseOverBorder.Background;
            TNewTagForeground = _ui.tNewTagForegroundBorder.Background;
            TTagButtonsBrush = _ui.tTagButtonsBorder.Background;
            TTagButtonsMouseOverBrush = _ui.tTagButtonsMouseOverBorder.Background;
            TTagsBrush = _ui.tTagsBorder.Background;
            TTagsMouseOverBrush = _ui.tTagsMouseOverBorder.Background;
            TForeground = _ui.tForegroundBorder.Background;

            foreach (IPreference preference in Items)
            {
                preference.Apply();
            }
        }

        public void Save(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(InterfaceMainWindowPreference));

            writer.WriteAttributeString("type", "preference");
            writer.WriteAttributeString("nativityInformationFontSize", NativityInformationFontSize.ToString());
            writer.WriteAttributeString("nativityInformationFontFamily", NativityInformationFontFamily.ToString());
            writer.WriteAttributeString("TButtonsBrush", TButtonsBrush.ToString());
            writer.WriteAttributeString("TButtonsMouseOverBrush", TButtonsMouseOverBrush.ToString());
            writer.WriteAttributeString("TEditingButtonsBrush", TEditingButtonsBrush.ToString());
            writer.WriteAttributeString("TEditingButtonsMouseOverBrush", TEditingButtonsMouseOverBrush.ToString());
            writer.WriteAttributeString("TEditingTagsBrush", TEditingTagsBrush.ToString());
            writer.WriteAttributeString("TEditingTagsMouseOverBrush", TEditingTagsMouseOverBrush.ToString());
            writer.WriteAttributeString("TEditingForeground", TEditingForeground.ToString());
            writer.WriteAttributeString("TEditingMouseOverForeground", TEditingMouseOverForeground.ToString());
            writer.WriteAttributeString("TMovingTagsBorderBrush", TMovingTagsBorderBrush.ToString());
            writer.WriteAttributeString("TNewTagBrush", TNewTagBrush.ToString());
            writer.WriteAttributeString("TNewTagButtonsBrush", TNewTagButtonsBrush.ToString());
            writer.WriteAttributeString("TNewTagButtonsMouseOverBrush", TNewTagButtonsMouseOverBrush.ToString());
            writer.WriteAttributeString("TNewTagForeground", TNewTagForeground.ToString());
            writer.WriteAttributeString("TTagButtonsBrush", TTagButtonsBrush.ToString());
            writer.WriteAttributeString("TTagButtonsMouseOverBrush", TTagButtonsMouseOverBrush.ToString());
            writer.WriteAttributeString("TTagsBrush", TTagsBrush.ToString());
            writer.WriteAttributeString("TTagsMouseOverBrush", TTagsMouseOverBrush.ToString());
            writer.WriteAttributeString("TForeground", TForeground.ToString());

            //Save Items
            foreach (IPreference preference in Items)
            {
                preference.Save(writer);
            }

            writer.WriteEndElement();
        }

        public void Reset()
        {
            InterfaceMainWindowPreferenceUI _ui = (InterfaceMainWindowPreferenceUI)ui;
            _ui.nativityInformationFontSizeNumericUpDown.Value = Convert.ToDouble(NativityInformationFontSizeProperty.DefaultMetadata.DefaultValue);
            _ui.nativityInformationFontFamilyComboBox.Text = NativityInformationFontFamilyProperty.DefaultMetadata.DefaultValue.ToString();
            _ui.tButtonsBorder.Background = (Brush)TButtonsBrushProperty.DefaultMetadata.DefaultValue;
            _ui.tButtonsMouseOverBorder.Background = (Brush)TButtonsMouseOverBrushProperty.DefaultMetadata.DefaultValue;
            _ui.tEditingButtonsBorder.Background = (Brush)TEditingButtonsBrushProperty.DefaultMetadata.DefaultValue;
            _ui.tEditingButtonsMouseOverBorder.Background = (Brush)TEditingButtonsMouseOverBrushProperty.DefaultMetadata.DefaultValue;
            _ui.tEditingTagsBorder.Background = (Brush)TEditingTagsBrushProperty.DefaultMetadata.DefaultValue;
            _ui.tEditingTagsMouseOverBorder.Background = (Brush)TEditingTagsMouseOverBrushProperty.DefaultMetadata.DefaultValue;
            _ui.tEditingForegroundBorder.Background = (Brush)TEditingForegroundProperty.DefaultMetadata.DefaultValue;
            _ui.tEditingMouseOverForegroundBorder.Background = (Brush)TEditingMouseOverForegroundProperty.DefaultMetadata.DefaultValue;
            _ui.tMovingTagsBorder.Background = (Brush)TMovingTagsBorderBrushProperty.DefaultMetadata.DefaultValue;
            _ui.tNewTagBorder.Background = (Brush)TNewTagBrushProperty.DefaultMetadata.DefaultValue;
            _ui.tNewTagButtonsBorder.Background = (Brush)TNewTagButtonsBrushProperty.DefaultMetadata.DefaultValue;
            _ui.tNewTagButtonsMouseOverBorder.Background = (Brush)TNewTagButtonsMouseOverBrushProperty.DefaultMetadata.DefaultValue;
            _ui.tNewTagForegroundBorder.Background = (Brush)TNewTagForegroundProperty.DefaultMetadata.DefaultValue;
            _ui.tTagButtonsBorder.Background = (Brush)TTagButtonsBrushProperty.DefaultMetadata.DefaultValue;
            _ui.tTagButtonsMouseOverBorder.Background = (Brush)TTagButtonsMouseOverBrushProperty.DefaultMetadata.DefaultValue;
            _ui.tTagsBorder.Background = (Brush)TTagsBrushProperty.DefaultMetadata.DefaultValue;
            _ui.tTagsMouseOverBorder.Background = (Brush)TTagsMouseOverBrushProperty.DefaultMetadata.DefaultValue;
            _ui.tForegroundBorder.Background = (Brush)TForegroundProperty.DefaultMetadata.DefaultValue;
        }

        public void ResetAll()
        {
            Reset();
            foreach (IPreference preference in Items)
            {
                preference.ResetAll();
            }
        }

        public UIElement ShowUI()
        {
            object o = this;
            return ui;
        }

        public void CloseUI(bool recrusive)
        {
            ui = new InterfaceMainWindowPreferenceUI(this);

            if (recrusive)
            {
                foreach (IPreference preference in Items)
                {
                    preference.CloseUI(recrusive);
                }
            }
        }

        #endregion


        #region Private Methods

        private void reloadUI()
        {
            ui = new InterfaceMainWindowPreferenceUI(this);
        }

        #endregion


        #region Static Methods

        public static InterfaceMainWindowPreference Load(XmlNode node)
        {
            InterfaceMainWindowPreference preference = new InterfaceMainWindowPreference();

            foreach (XmlAttribute attribute in node.Attributes)
            {
                if (attribute.LocalName == "nativityInformationFontSize")
                    preference.NativityInformationFontSize = Convert.ToDouble(attribute.Value);
                else if (attribute.LocalName == "nativityInformationFontFamily")
                    preference.NativityInformationFontFamily = new FontFamily(attribute.Value);
                else if (attribute.LocalName == "TButtonsBrush")
                    preference.TButtonsBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(attribute.Value));
                else if (attribute.LocalName == "TButtonsMouseOverBrush")
                    preference.TButtonsMouseOverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(attribute.Value));
                else if (attribute.LocalName == "TEditingButtonsBrush")
                    preference.TEditingButtonsBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(attribute.Value));
                else if (attribute.LocalName == "TEditingButtonsMouseOverBrush")
                    preference.TEditingButtonsMouseOverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(attribute.Value));
                else if (attribute.LocalName == "TEditingTagsBrush")
                    preference.TEditingTagsBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(attribute.Value));
                else if (attribute.LocalName == "TEditingTagsMouseOverBrush")
                    preference.TEditingTagsMouseOverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(attribute.Value));
                else if (attribute.LocalName == "TEditingForeground")
                    preference.TEditingForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(attribute.Value));
                else if (attribute.LocalName == "TEditingMouseOverForeground")
                    preference.TEditingMouseOverForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(attribute.Value));
                else if (attribute.LocalName == "TMovingTagsBorderBrush")
                    preference.TMovingTagsBorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(attribute.Value));
                else if (attribute.LocalName == "TNewTagBrush")
                    preference.TNewTagBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(attribute.Value));
                else if (attribute.LocalName == "TNewTagButtonsBrush")
                    preference.TNewTagButtonsBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(attribute.Value));
                else if (attribute.LocalName == "TNewTagButtonsMouseOverBrush")
                    preference.TNewTagButtonsMouseOverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(attribute.Value));
                else if (attribute.LocalName == "TNewTagForeground")
                    preference.TNewTagForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(attribute.Value));
                else if (attribute.LocalName == "TTagButtonsBrush")
                    preference.TTagButtonsBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(attribute.Value));
                else if (attribute.LocalName == "TTagButtonsMouseOverBrush")
                    preference.TTagButtonsMouseOverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(attribute.Value));
                else if (attribute.LocalName == "TTagsBrush")
                    preference.TTagsBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(attribute.Value));
                else if (attribute.LocalName == "TTagsMouseOverBrush")
                    preference.TTagsMouseOverBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(attribute.Value));
                else if (attribute.LocalName == "TForeground")
                    preference.TForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(attribute.Value));
            }

            preference.reloadUI();

            foreach (XmlNode child in node.ChildNodes)
            {
                preference.Items.Add(Preferences.LoadPreference(child));
            }

            return preference;
        }

        #endregion


    }
}