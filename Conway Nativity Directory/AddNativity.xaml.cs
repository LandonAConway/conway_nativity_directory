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
using System.Windows.Shapes;

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for AddNativity.xaml
    /// </summary>
    public partial class AddNativity : Window
    {


        #region AddNativity

        public AddNativity()
        {
            InitializeComponent();
            this.Closing += AddNativity_Closing;
            ConfigureNativity();

            int id = 1;
            var sorted = App.Project.Nativities.Cast<Nativity>().ToList().OrderBy(x => x.Id);
            var lastNativity = sorted.LastOrDefault();
            if (lastNativity != null)
                id = lastNativity.Id + 1;
            idTextBox.Text = id.ToString();
        }

        #endregion


        #region Nativity

        public readonly Nativity Nativity = new Nativity();

        private void ConfigureNativity()
        {
            //App.SetBinding("Text", idTextBox, Nativity, Nativity.IdProperty);
            //App.SetBinding("Text", titleTextBox, Nativity, Nativity.TitleProperty);
            //App.SetBinding("Text", originTextBox, Nativity, Nativity.OriginProperty);
            //App.SetBinding("Text", acquiredTextBox, Nativity, Nativity.AcquiredProperty);
            //App.SetBinding("Text", fromTextBox, Nativity, Nativity.FromProperty);
            //App.SetBinding("Text", costTextBox, Nativity, Nativity.CostProperty);
            //App.SetBinding("Text", locationTextBox, Nativity, Nativity.LocationProperty);
            //App.SetBinding("Text", descriptionTextBox, Nativity, Nativity.DescriptionProperty);

            Nativity.Tags = new System.Collections.ObjectModel.ObservableCollection<string>();
            Nativity.GeographicalOrigins = new System.Collections.ObjectModel.ObservableCollection<string>();
        }

        #endregion


        #region Other

        private bool successful = false;
        public bool Successful { get { return successful; } }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            successful = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            successful = false;
            this.Close();
        }

        private void AddNativity_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Nativity.Id = Convert.ToInt32(idTextBox.Text);
            Nativity.Title = titleTextBox.Text;
            Nativity.Origin = originTextBox.Text;
            Nativity.Acquired = acquiredTextBox.Text;
            Nativity.From = fromTextBox.Text;
            Nativity.Cost = Convert.ToDouble((String.IsNullOrEmpty(costTextBox.Text) || String.IsNullOrWhiteSpace(costTextBox.Text))
                ? "0" : costTextBox.Text);
            Nativity.Location = locationTextBox.Text;
            Nativity.Description = descriptionTextBox.Text;
        }


        #endregion


        #region Nativity Parsing

        private void parseButton_Click(object sender, RoutedEventArgs e)
        {
            string text = System.Windows.Clipboard.GetText();
            string[] data = ParseData(text);

            if (data != null)
            {
                idTextBox.Text = data[0];
                titleTextBox.Text = data[1];
                descriptionTextBox.Text = data[2];
            }
        }

        private string[] ParseData(string text)
        {
            string _text = text;

            //clean start
            _text = _text.Trim();
            if (_text.StartsWith("no.", StringComparison.InvariantCultureIgnoreCase))
                _text = _text.Substring(3);
            else if (_text.StartsWith("no", StringComparison.InvariantCultureIgnoreCase))
                _text = _text.Substring(2);

            //separate into lines
            string[] lines = _text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            //separate index and title
            string index = new string(lines[0].TakeWhile(Char.IsDigit).ToArray());

            var regex = new Regex(Regex.Escape(index));
            var title = regex.Replace(lines[0], String.Empty, 1);
            
            if (title.StartsWith("."))
                title = title.Substring(1);

            //get description
            List<string> theRest = new List<string>();
            foreach (string line in lines)
            {
                if (line != lines[0])
                    theRest.Add(line);
            }

            string description = String.Join(Environment.NewLine, theRest);

            //trim whitespace
            title = title.Trim();
            description = description.Trim();

            return new string[] { index, title, description };
        }

        #endregion


    }
}
