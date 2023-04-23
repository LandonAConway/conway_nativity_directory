using System;
using System.Collections.Generic;
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
using System.IO;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using ConwayNativityDirectory.PluginApi;
using System.Windows.Forms.VisualStyles;

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for ImportNarrativesDialog.xaml
    /// </summary>
    public partial class ImportNarrativesDialog : Window
    {
        public ImportNarrativesDialog()
        {
            InitializeComponent();
            cancelButton.Click += CancelButton_Click;
            okButton.Click += OkButton_Click;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            ImportNarratives();
        }

        private void ImportNarratives()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Documents (*.txt)|*.txt";
            if (ofd.ShowDialog() == true)
            {
                WaitForm waitForm = new WaitForm("Please Wait...");
                waitForm.Text = "Parse Narratives";
                waitForm.Show();
                waitForm.Update();

                //string[] content = File.ReadAllLines(ofd.FileName, Encoding.UTF8);

                //int id = -1;
                //foreach (string line in content)
                //{
                //    if (!String.IsNullOrWhiteSpace(line) && Char.IsDigit(line.FirstOrDefault()))
                //    {
                //        string number = "";
                //        foreach (char c in line)
                //        {
                //            if (Char.IsDigit(c))
                //                number = number + c.ToString();
                //            else
                //                break;
                //        }
                //        id = Convert.ToInt32(number);
                //        break;
                //    }
                //}

                //if (id == -1)
                //{
                //    MessageBox.Show("Could not parse nativities. Are you sure the file is in the correct format?");
                //    return;
                //}

                //List<Nativity> nativities = new List<Nativity>(); bool createDuplicate = false;
                //int index = 0;
                //string title = "";
                //string desciption = "";
                //foreach (string line in content)
                //{
                //    try
                //    {
                //        if (line.StartsWith(id.ToString() + "."))
                //        {
                //            if (index > 0)
                //            {
                //                if (createDuplicate)
                //                {
                //                    nativities[index - 1].Description = desciption.TrimStart('\n').TrimEnd('\n');
                //                    nativities[index].Description = desciption.TrimStart('\n').TrimEnd('\n');
                //                    createDuplicate = false;
                //                }

                //                else
                //                    nativities[index - 1].Description = desciption.TrimStart('\n').TrimEnd('\n');
                //            }

                //            index++;
                //            title = line.Substring(id.ToString().Length + 1).TrimStart(' ').TrimEnd(' ');
                //            desciption = "";
                //            nativities.Add(new Nativity() { Id = id, Title = title });

                //            id++;
                //        }

                //        else if (line.StartsWith(id.ToString() + "-" + (id + 1).ToString() + "."))
                //        {
                //            if (index > 0)
                //            {
                //                if (createDuplicate)
                //                {
                //                    nativities[index - 2].Description = desciption.TrimStart('\n').TrimEnd('\n');
                //                    nativities[index - 1].Description = desciption.TrimStart('\n').TrimEnd('\n');
                //                    createDuplicate = false;
                //                }

                //                else
                //                    nativities[index - 1].Description = desciption.TrimStart('\n').TrimEnd('\n');
                //            }

                //            index = id;
                //            title = line.Substring(id.ToString().Length + (id + 1).ToString().Length + 2).TrimStart(' ').TrimEnd(' ');
                //            desciption = "";
                //            createDuplicate = true;

                //            nativities.Add(new Nativity() { Id = id, Title = title });
                //            nativities.Add(new Nativity() { Id = id + 1, Title = title });

                //            id = id + 2;
                //        }

                //        else
                //        {
                //            desciption = desciption + line;
                //        }
                //    }

                //    catch
                //    {
                //        MessageBox.Show("There was an issue parsing the rest of the file. Please make sure that each nativity's id & title has it's " +
                //            "own line followed by a period and a description below. (Do not add multiple id's in a single line before a period. " +
                //            "i.e. \"1-2. Title\". Nativity 1 and 2 in the example must be on a seperate line with their own title.)");
                //    }
                //}

                //var lastNativity = nativities.LastOrDefault();
                //if (lastNativity != null)
                //    lastNativity.Description = desciption;

                string text = File.ReadAllText(ofd.FileName, Encoding.UTF8);

                NativityTextParser parser = NativityTextParser.Create(text);
                parser.Parse();

                List<Nativity> nativities = new List<Nativity>();
                foreach (string[] x in parser.Output)
                {
                    Nativity nativity = new Nativity();
                    nativity.Id = Convert.ToInt32(x[0]);
                    nativity.Title = x[1];
                    nativity.Description = x[2];
                    nativity.Tags = new System.Collections.ObjectModel.ObservableCollection<string>();
                    nativity.GeographicalOrigins = new System.Collections.ObjectModel.ObservableCollection<string>();
                    nativities.Add(nativity);
                }

                waitForm.Close();

                if (nativities.Count > 0)
                {
                    MessageBoxResult mbr = MessageBox.Show("The parser stopped reading at nativity #" + nativities.Last().Id + ". Would you like to continue?",
                        "Parse Narratives", MessageBoxButton.YesNo);
                    if (mbr == MessageBoxResult.Yes)
                    {
                        foreach (Nativity nativity in nativities)
                        {
                            var currentNativities = App.Project.Nativities.ToList().Cast<Nativity>();
                            //App.Project.Nativities.Add(nativity);
                            if (currentNativities.Where(a => a.Id == nativity.Id).Count() > 0)
                            {
                                var target = currentNativities.Where(a => a.Id == nativity.Id).FirstOrDefault();
                                if (addRadioButton.IsChecked == true)
                                    App.Project.Nativities.Add(nativity);
                                else if (updateRadioButton.IsChecked == true)
                                {
                                    target.Title = nativity.Title;
                                    target.Description = nativity.Description;
                                }
                            }

                            else if (addNativitiesWithNewIdsCheckBox.IsChecked == true)
                                App.Project.Nativities.Add(nativity);
                        }

                        MessageBox.Show("The operation was successful.");
                    }
                }

                else
                {
                    MessageBox.Show("No nativities were parsed.");
                }
            }
        }
    }
}
