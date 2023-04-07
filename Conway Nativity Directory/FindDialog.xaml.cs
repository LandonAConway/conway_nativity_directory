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
using System.Windows.Threading;
using System.Globalization;

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for FindDialog.xaml
    /// </summary>
    public partial class FindDialog : Window
    {
        MainWindow mainWindow;
        public FindDialog()
        {
            InitializeComponent();
            mainWindow = App.MainWindow;
            this.Top = (mainWindow.ActualHeight / 8) * 1;
            this.Left = (mainWindow.ActualWidth / 5) * 3;

            lastNativityCount = mainWindow.nativityListView.Items.Count;
            currentNativity = mainWindow.nativityListView.SelectedIndex;
            if (currentNativity < 0)
                currentNativity++;

            mainWindow.nativityListView.SelectionChanged += NativityListView_SelectionChanged;
        }

        public new void ShowDialog()
        {
            base.ShowDialog();
            _ = FocusTextBoxAsync();
        }

        public new void Show()
        {
            base.Show();
            _ = FocusTextBoxAsync();
        }

        private async Task FocusTextBoxAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(0.25));
            await Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new Action(delegate () {
                    searchForTextBox.Focus();
                    searchForTextBox.CaretIndex = searchForTextBox.Text.Count();
                    Keyboard.Focus(searchForTextBox);
                }));
        }

        private void NativityListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = mainWindow.nativityListView.SelectedIndex;

            if (selectedIndex != currentNativity)
            {
                if (selectedIndex < 0)
                    resetToMinimum(true);
                else
                {
                    resetToMinimum(true);
                    currentNativity = selectedIndex;
                }
            }
        }

        private void resetToMinimum(bool resetMatches = false)
        {
            currentNativity = 0;
            currentNativityProperty = 1;
            currentMatch = 0;

            if (resetMatches)
                matches = 0;
        }

        int lastNativityCount = 0;
        private void handleNativityCountChange()
        {
            if (lastNativityCount != mainWindow.nativityListView.Items.Count)
                resetToMinimum(true);
        }

        int maxProperty = 9;
        int currentNativity = 0;
        int currentNativityProperty = 1;
        int currentMatch = 0;
        int matches = 0;

        private void findNextBtn_Click(object sender, RoutedEventArgs e)
        {
            handleNativityCountChange();

            int _currentNativity = 0;
            int _currentNativityProperty = 1;
            int _currentMatch = 0;

            int nativityCount = mainWindow.nativityListView.Items.Count;
            lastNativityCount = nativityCount;
            bool finishedAll = false;

            refreshCaseSensitive();
            if (nativityCount > 0)
            {
                bool finished = false;

                do
                {
                    PredictMatchInfo();

                    if (currentMatch == -100)
                        currentMatch = matches - 1;

                    if (matches > 0)
                    {
                        finished = true;
                        _currentNativity = currentNativity;
                        _currentNativityProperty = currentNativityProperty;
                        _currentMatch = currentMatch;
                    }

                    currentMatch++;

                    if (currentMatch > (matches - 1))
                    {
                        currentNativityProperty++;

                        if (currentNativityProperty > maxProperty)
                        {
                            currentNativity++;

                            if (currentNativity > (nativityCount - 1))
                            {
                                currentNativity = 0;
                                currentNativityProperty = 1;
                                currentMatch = 0;
                                finishedAll = true;
                                finished = true;
                            }

                            currentNativityProperty = 1;
                        }

                        currentMatch = 0;
                    }
                }

                while (!finished);
            }

            if (matches > 0)
                showResult(_currentNativity, _currentNativityProperty, _currentMatch);

            else if (finishedAll)
                MessageBox.Show("Nothing was found");

            //to use for debugging...
            //MessageBox.Show("nativity index:" + currentNativity.ToString() + ", property:" +
            //    currentNativityProperty.ToString() + ", matches:" +
            //    matches.ToString() + ", match index:" + currentMatch.ToString());
        }

        private void findPreviousBtn_Click(object sender, RoutedEventArgs e)
        {
            handleNativityCountChange();

            int _currentNativity = 0;
            int _currentNativityProperty = 1;
            int _currentMatch = 0;

            int nativityCount = mainWindow.nativityListView.Items.Count;
            lastNativityCount = nativityCount;
            bool finishedAll = false;

            refreshCaseSensitive();
            if (nativityCount > 0)
            {
                bool finished = false;

                do
                {
                    PredictMatchInfo();

                    if (currentMatch == -100)
                        currentMatch = matches - 1;

                    if (matches > 0)
                    {
                        finished = true;
                        _currentNativity = currentNativity;
                        _currentNativityProperty = currentNativityProperty;
                        _currentMatch = currentMatch;
                    }

                    currentMatch--;

                    if (currentMatch < 0)
                    {
                        currentNativityProperty--;

                        if (currentNativityProperty < 1)
                        {
                            currentNativity--;

                            if (currentNativity < 0)
                            {
                                currentNativity = nativityCount - 1;
                                currentNativityProperty = maxProperty;
                                currentMatch = -100;
                                finishedAll = true;
                                finished = true;
                            }

                            currentNativityProperty = 7;
                        }

                        currentMatch = -100;
                    }
                }

                while (!finished);
            }

            if (matches > 0)
                showResult(_currentNativity, _currentNativityProperty, _currentMatch);

            else if (finishedAll)
                MessageBox.Show("Nothing was found");
        }

        private DependencyProperty getNativityProperty(int propertyIndex)
        {
            if (propertyIndex == 1)
                return Nativity.IdProperty;

            else if (propertyIndex == 2)
                return Nativity.TitleProperty;

            else if (propertyIndex == 3)
                return Nativity.OriginProperty;

            else if (propertyIndex == 4)
                return Nativity.AcquiredProperty;

            else if (propertyIndex == 5)
                return Nativity.FromProperty;

            else if (propertyIndex == 6)
                return Nativity.CostProperty;

            else if (propertyIndex == 7)
                return Nativity.LocationProperty;

            else if (propertyIndex == 8)
                return Nativity.TagsProperty;

            else if (propertyIndex == 9)
                return Nativity.GeographicalOriginsProperty;

            return null;
        }

        private void showResult(int _currentNativity, int _currentNativityProperty, int _currentMatch)
        {
            var nativity = (Nativity)mainWindow.nativityListView.Items[_currentNativity];
            var property = getNativityProperty(_currentNativityProperty);

            mainWindow.nativityListView.ScrollIntoView(nativity);
            ForceUIUpdate();

            TextBox textBox = getTextBox(nativity, property,
                _currentNativity, _currentNativityProperty, _currentMatch);

            if (textBox != null)
            {
                if (_currentNativityProperty < 8)
                {
                    var matches = Regex.Matches(textBox.Text, searchForTextBox.Text, getRegexOptions());

                    string searchIn = textBox.Text;
                    string find = searchForTextBox.Text;
                    int match = _currentMatch;

                    int start = matches[match].Index;

                    Keyboard.Focus(textBox);

                    textBox.SelectionStart = start;
                    textBox.SelectionLength = find.Length;
                }

                else
                {
                    string find = searchForTextBox.Text;
                    int match = _currentMatch;

                    int start = matchInTag;

                    Keyboard.Focus(textBox);

                    textBox.SelectionStart = start;
                    textBox.SelectionLength = find.Length;
                }
            }
        }

        private void PredictMatchInfo()
        {
            Nativity nativity = mainWindow.Project.Nativities[currentNativity] as Nativity;
            string text = String.Empty;
            DependencyProperty dp = getNativityProperty(currentNativityProperty);

            if (currentNativityProperty == 1)
                text = nativity.Id.ToString();

            else if (currentNativityProperty == 2)
                text = nativity.Title;

            else if (currentNativityProperty == 3)
                text = nativity.Origin;

            else if (currentNativityProperty == 4)
                text = nativity.Acquired;

            else if (currentNativityProperty == 5)
                text = nativity.From;

            else if (currentNativityProperty == 6)
                text = nativity.Cost.ToString();

            else if (currentNativityProperty == 7)
                text = nativity.Location;

            else if (currentNativityProperty == 8)
                text = String.Join(String.Empty, nativity.Tags);

            else if (currentNativityProperty == 9)
                text = String.Join(String.Empty, nativity.GeographicalOrigins);

            if (text.Contains(searchForTextBox.Text, getCompareOptions()))
                matches = Regex.Matches(text, searchForTextBox.Text, getRegexOptions()).Count;
            else
                matches = 0;
        }

        private TextBox getTextBox(Nativity nativity, DependencyProperty property,
            int _currentNativity, int _currentNativityProperty, int _currentMatch)
        {
            TextBox textBox = null;

            if (property.Name == "Id")
                textBox = NativityColumnElements.GetIdColumn(nativity) as MaskedTextBox;
            else if (property.Name == "Title")
                textBox = NativityColumnElements.GetTitleColumn(nativity) as MaskedTextBox;
            else if (property.Name == "Origin")
                textBox = NativityColumnElements.GetOriginColumn(nativity) as MaskedTextBox;
            else if (property.Name == "Acquired")
                textBox = NativityColumnElements.GetAcquiredColumn(nativity) as MaskedTextBox;
            else if (property.Name == "From")
                textBox = NativityColumnElements.GetFromColumn(nativity) as MaskedTextBox;
            else if (property.Name == "Cost")
                textBox = NativityColumnElements.GetCostColumn(nativity) as MaskedTextBox;
            else if (property.Name == "Location")
                textBox = NativityColumnElements.GetLocationColumn(nativity) as MaskedTextBox;
            else if (property.Name == "Tags")
                textBox = getTextBoxFromTags(_currentNativity, _currentNativityProperty, _currentMatch);
            else if (property.Name == "GeographicalOrigins")
                textBox = getTextBoxFromTags(_currentNativity, _currentNativityProperty, _currentMatch);

            return textBox;
        }

        private TagEditor getTagEditor(Nativity nativity, DependencyProperty property)
        {
            TagEditor tagEditor = null;

            if (property.Name == "Tags")
                tagEditor = NativityColumnElements.GetTagsColumn(nativity) as TagEditor;
            else if (property.Name == "GeographicalOrigins")
                tagEditor = NativityColumnElements.GetGeographicalOriginsColumn(nativity) as TagEditor;

            return tagEditor;
        }

        int matchInTag = 0;
        private int getCurrentTagIndex(int _currentNativity, int _currentNativityProperty, int _currentMatch)
        {
            var property = getNativityProperty(_currentNativityProperty);
            var nativity = (Nativity)mainWindow.nativityListView.Items[_currentNativity];

            List<string> tags = null;
            if (property == Nativity.TagsProperty)
                tags = nativity.Tags.ToList();
            else if (property == Nativity.GeographicalOriginsProperty)
                tags = nativity.GeographicalOrigins.ToList();

            string text = string.Join(String.Empty, tags);

            var matches = Regex.Matches(text, searchForTextBox.Text, getRegexOptions());
            var targetMatch = matches[_currentMatch];

            int lengthBefore = 0;
            int tagCounter = 0;

            foreach (string tag in tags)
            {
                if (targetMatch.Index < lengthBefore + tag.Length)
                {
                    matchInTag = targetMatch.Index - lengthBefore;
                    break;
                }

                tagCounter++;
                lengthBefore += tag.Length;
            }

            return tagCounter;

            ////resources
            //string[] tags = { "hello", "hi", "thiandis", "0i1i101i1i1" };
            //string joined = string.Join("", tags);
            //int currentMatch = 4; //The 'i' in "this"

            ////
            //var matches = Regex.Matches(joined, "i");
            //var targetMatch = matches[currentMatch];

            //int indexOfTag = 0;
            //int tagCount = 0;
            //int tagMatchIndex = 0;
            //foreach (string tag in tags)
            //{
            //    if (targetMatch.Index < indexOfTag + tag.Length)
            //    {
            //        tagMatchIndex = targetMatch.Index - indexOfTag;
            //        break;
            //    }

            //    tagCount++;
            //    indexOfTag += tag.Length;
            //}

            //Console.WriteLine("Tag index: " + tagCount + ", Index of match: " + tagMatchIndex);
            //Console.ReadLine();
        }

        private TextBox getTextBoxFromTags(int _currentNativity, int _currentNativityProperty, int _currentMatch)
        {
            var nativity = (Nativity)mainWindow.nativityListView.Items[currentNativity];
            int currentTag = getCurrentTagIndex(_currentNativity, _currentNativityProperty, _currentMatch);
            TagEditor tagEditor = null;

            if (_currentNativityProperty == 8)
                tagEditor = NativityColumnElements.GetTagsColumn(nativity) as TagEditor;
            else if (_currentNativityProperty == 9)
                tagEditor = NativityColumnElements.GetGeographicalOriginsColumn(nativity) as TagEditor;

            List<Tag> _tagControls = new List<Tag>(tagEditor.tagControls);
            List<Tag> tagControls = new List<Tag>();
            foreach (string tag in tagEditor.TagsSource)
            {
                Tag tc = _tagControls.Where(t => t.Text == tag).FirstOrDefault();
                if (tc != null)
                {
                    _tagControls.Remove(tc);
                    tagControls.Add(tc);
                }
            }

            var textBoxes = tagControls.Select(t => t.textBox).ToList();
            return textBoxes.ElementAtOrDefault(currentTag);
        }

        private void ForceUIUpdate()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Render,
                new DispatcherOperationCallback(delegate (object parameter)
                {
                    frame.Continue = false;
                    return null;
                }), null);

            Dispatcher.PushFrame(frame);
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                new Action(delegate { }));
        }

        bool caseSensitive = false;
        private void refreshCaseSensitive()
        {
            caseSensitive = caseSensitiveCheckBox.IsChecked.Value;
            PredictMatchInfo();

            if (currentMatch > matches)
                currentMatch = matches - 1;

            if (currentMatch < 0)
                currentMatch = 0;
        }

        private CompareOptions getCompareOptions()
        {
            if (caseSensitive)
                return CompareOptions.None;
            else
                return CompareOptions.IgnoreCase;
        }

        private StringComparison getStringComparison()
        {
            if (caseSensitive)
                return StringComparison.Ordinal;
            else
                return StringComparison.OrdinalIgnoreCase;
        }

        private RegexOptions getRegexOptions()
        {
            if (caseSensitive)
                return RegexOptions.None;
            else
                return RegexOptions.IgnoreCase;
        }
    }
}
