using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for ScriptEditorWindow.xaml
    /// </summary>
    public partial class ScriptEditorWindow : Window
    {
        public ScriptEditorWindow()
        {
            InitializeComponent();
            System.IO.StreamReader stream = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\bin\lua\editor\syntax.xshd");
            System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(stream);
            textEditor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(reader,
                ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
        }

        public string Code 
        {
            get => textEditor.Text;
            set => textEditor.Text = value;
        }

        bool result = false;
        public new bool ShowDialog()
        {
            base.ShowDialog();
            return result;
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            result = true;
            this.Close();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
