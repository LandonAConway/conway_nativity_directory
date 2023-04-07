using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    /// Interaction logic for ScriptsManagerWindow.xaml
    /// </summary>
    public partial class ScriptsManagerWindow : Window
    {
        public ScriptsManagerWindow()
        {
            InitializeComponent();
            Scripts = new ObservableCollection<ScriptItem>();
            LoadScripts();
        }


        public static readonly DependencyProperty ScriptsProperty = DependencyProperty.Register(
            nameof(Scripts), typeof(ObservableCollection<ScriptItem>), typeof(ScriptsManagerWindow),
            new PropertyMetadata(new ObservableCollection<ScriptItem>()));

        public ObservableCollection<ScriptItem> Scripts
        {
            get { return (ObservableCollection<ScriptItem>)GetValue(ScriptsProperty); }
            set { SetValue(ScriptsProperty, value); }
        }


        private void importBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Lua Script (*.lua)|*.lua";
            if (ofd.ShowDialog() == true)
            {
                string copiedFileName = AppDomain.CurrentDomain.BaseDirectory + @"\bin\lua\scripts\" +
                    System.IO.Path.GetFileName(ofd.FileName);
                if (System.IO.File.Exists(copiedFileName))
                {
                    MessageBox.Show("This script already exists. Change the name, then try to import it again.");
                    return;
                }
                System.IO.File.Copy(ofd.FileName, copiedFileName);
                var script = new ScriptItem();
                script.FileName = ofd.FileName;
                Scripts.Add(script);
            }
        }

        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show("Are you sure you want to remove the selected scripts? This action cannot be undone.",
                    "Scripts Manager", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mbr == MessageBoxResult.Yes)
            {
                foreach (ScriptItem script in scriptsListView.SelectedItems.Cast<ScriptItem>().ToList())
                {
                    System.IO.File.Delete(script.FileName);
                    Scripts.Remove(script);
                }
            }
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            if (scriptsListView.SelectedItems.Count > 0)
            {
                ScriptEditorWindow csw = new ScriptEditorWindow();
                string fileName = (scriptsListView.SelectedItem as ScriptItem).FileName;
                csw.Code = System.IO.File.ReadAllText(fileName);
                bool result = csw.ShowDialog();
                if (result)
                    System.IO.File.WriteAllText(fileName, csw.Code);
            }
        }

        private void createBtn_Click(object sender, RoutedEventArgs e)
        {
            ScriptEditorWindow csw = new ScriptEditorWindow();
            bool result = csw.ShowDialog();
            if (result)
                CreateScript(csw.Code);
        }

        private void CreateScript(string code)
        {
            InputDialog inputDialog = new InputDialog();
            inputDialog.Title = "Create Script Title";
            if (inputDialog.ShowDialog() == true)
            {
                string fileName = AppDomain.CurrentDomain.BaseDirectory + @"\bin\lua\scripts\" + inputDialog.Response + ".lua";
                if (IsValidFileName(System.IO.Path.GetFileName(fileName)) && !String.IsNullOrWhiteSpace(inputDialog.Response))
                {
                    if (!Scripts.Where(x => x.FileName == fileName).Any())
                    {
                        System.IO.File.WriteAllText(fileName, code);
                        Scripts.Add(new ScriptItem() { FileName = fileName });
                    }
                    else
                    {
                        MessageBox.Show("A script with the title \"" + inputDialog.Response + "\" already exists.");
                        CreateScript(code);
                    }
                }

                else
                {
                    MessageBox.Show("The script's title is not valid.");
                    CreateScript(code);
                }
            }
        }

        private bool IsValidFileName(string path)
        {
            foreach (char c in path)
            {
                if ("<>:\"/\\|?*".Contains(c.ToString()))
                    return false;
            }
            return true;
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            SetScripts();
            this.Close();
        }

        private void LoadScripts()
        {
            foreach (var item in App.MainWindow.scriptsMenuItem.Items.Cast<object>().Where(x => x is ScriptMenuItem).Cast<ScriptMenuItem>())
            {
                var script = new ScriptItem();
                script.FileName = item.FileName;
                Scripts.Add(script);
            }
        }

        private void SetScripts()
        {
            foreach (var item in App.MainWindow.scriptsMenuItem.Items.Cast<object>().Where(x => x is ScriptMenuItem).ToArray())
                App.MainWindow.scriptsMenuItem.Items.Remove(item);
            var hasSeparator = App.MainWindow.scriptsMenuItem.Items.Cast<object>().Where(x => x is Separator).Any();
            if (!hasSeparator)
                App.MainWindow.scriptsMenuItem.Items.Add(new Separator());
            foreach (var item in Scripts)
                App.MainWindow.scriptsMenuItem.Items.Add(new ScriptMenuItem() { FileName = item.FileName });
        }
    }
}
