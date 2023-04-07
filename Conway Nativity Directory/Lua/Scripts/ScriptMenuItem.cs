using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Conway_Nativity_Directory
{
    public class ScriptMenuItem : MenuItem
    {
        public ScriptMenuItem()
        {
            try
            {
                this.Click += App.MainWindow.executeScriptMenuItem_Click;
            }
            catch { }
        }

        public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register(
            nameof(FileName), typeof(string), typeof(ScriptMenuItem),
            new PropertyMetadata(null, new PropertyChangedCallback(FileNameDependencyPropertyChanged)));

        public static void FileNameDependencyPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as ScriptMenuItem).SetTitle(System.IO.Path.GetFileNameWithoutExtension((string)e.NewValue));
            (sender as ScriptMenuItem).Header = (sender as ScriptMenuItem).Title;
        }

        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }


        private static readonly DependencyPropertyKey TitlePropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(Title), typeof(string), typeof(ScriptMenuItem),
            new PropertyMetadata());

        public static readonly DependencyProperty TitleProperty =
            TitlePropertyKey.DependencyProperty;

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
        }

        private void SetTitle(string value)
        {
            SetValue(TitlePropertyKey, value);
        }
    }
}
