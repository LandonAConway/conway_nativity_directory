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

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for AdvancedSearch.xaml
    /// </summary>
    public partial class AdvancedSearch : Window
    {
        public static readonly DependencyProperty HighlightWordsInDescriptionsProperty =
            DependencyProperty.Register(nameof(HighlightWordsInDescriptions), typeof(bool), typeof(AdvancedSearch), new PropertyMetadata(false));

        public bool HighlightWordsInDescriptions
        {
            get { return (bool)GetValue(HighlightWordsInDescriptionsProperty); }
            set { SetValue(HighlightWordsInDescriptionsProperty, value); }
        }

        public MainWindow mainWindow;
        public AdvancedSearch(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.termsListView.Items.Add(new SearchTerm { Text = "Search Term " + (this.termsListView.Items.Count+1).ToString() });
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            List<SearchTerm> searchTerms = new List<SearchTerm>();
            foreach (SearchTerm searchTerm in termsListView.Items)
            { searchTerms.Add(searchTerm); }

            foreach (SearchTerm searchTerm in searchTerms)
            {
                if (searchTerm.IsSelected)
                {
                    termsListView.Items.Remove(searchTerm);
                }
            }
        }

        private void EditColorButton_Click(object sender, RoutedEventArgs e)
        {
            List<SearchTerm> searchTerms = new List<SearchTerm>();
            foreach (SearchTerm searchTerm in termsListView.SelectedItems)
            {
                searchTerms.Add(searchTerm);
            }

            if (searchTerms.Count > 0)
            {
                System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
                if (searchTerms.Count == 1)
                {
                    Color color = ((SolidColorBrush)searchTerms[0].Brush).Color;
                    colorDialog.Color = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
                }

                colorDialog.ShowDialog();

                foreach (SearchTerm searchTerm in searchTerms)
                {
                    System.Drawing.Color dColor = colorDialog.Color;
                    Color color = Color.FromRgb(dColor.R, dColor.G, dColor.B);
                    searchTerm.Brush = new SolidColorBrush(color);
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HideButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Project.SetSearchFilter(new TextBoxFilter(mainWindow.searchTextBox));
            mainWindow.searchTextBox.Text = "";
            mainWindow.Project.Search();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (termsListView.Items.Count > 0)
            {
                mainWindow.Project.SetSearchFilter(new SearchTermCollectionFilter(new SearchTermCollection(termsListView)));
                mainWindow.Project.Search();
            }

            else
            {
                ClearButton_Click(null, null);
            }
        }
    }

    public class SearchTerm : DependencyObject
    {


        #region Dependency Properties

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.Register(nameof(IsEnabled), typeof(bool), typeof(SearchTerm), new PropertyMetadata(true));

        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }


        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(SearchTerm));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        public static readonly DependencyProperty BrushProperty =
            DependencyProperty.Register(nameof(Brush), typeof(Brush), typeof(SearchTerm), new PropertyMetadata(new SolidColorBrush(Colors.Gold)));

        public Brush Brush
        {
            get { return (Brush)GetValue(BrushProperty); }
            set { SetValue(BrushProperty, value); }
        }


        public static readonly DependencyProperty SearchForTextProperty =
            DependencyProperty.Register(nameof(SearchForText), typeof(bool), typeof(SearchTerm), new PropertyMetadata(true));

        public bool SearchForText
        {
            get { return (bool)GetValue(SearchForTextProperty); }
            set { SetValue(SearchForTextProperty, value); }
        }



        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(SearchTerm));

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        #endregion


        #region Search Categories

        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register(nameof(Id), typeof(bool), typeof(SearchTerm), new PropertyMetadata(true));

        public bool Id
        {
            get { return (bool)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }


        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(bool), typeof(SearchTerm), new PropertyMetadata(true));

        public bool Title
        {
            get { return (bool)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }


        public static readonly DependencyProperty OriginProperty =
            DependencyProperty.Register(nameof(Origin), typeof(bool), typeof(SearchTerm), new PropertyMetadata(true));

        public bool Origin
        {
            get { return (bool)GetValue(OriginProperty); }
            set { SetValue(OriginProperty, value); }
        }


        public static readonly DependencyProperty AcquiredProperty =
            DependencyProperty.Register(nameof(Acquired), typeof(bool), typeof(SearchTerm), new PropertyMetadata(true));

        public bool Acquired
        {
            get { return (bool)GetValue(AcquiredProperty); }
            set { SetValue(AcquiredProperty, value); }
        }


        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register(nameof(From), typeof(bool), typeof(SearchTerm), new PropertyMetadata(true));

        public bool From
        {
            get { return (bool)GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }


        public static readonly DependencyProperty CostProperty =
            DependencyProperty.Register(nameof(Cost), typeof(bool), typeof(SearchTerm), new PropertyMetadata(true));

        public bool Cost
        {
            get { return (bool)GetValue(CostProperty); }
            set { SetValue(CostProperty, value); }
        }


        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.Register(nameof(Location), typeof(bool), typeof(SearchTerm), new PropertyMetadata(true));

        public bool Location
        {
            get { return (bool)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }


        public static readonly DependencyProperty TagsProperty =
            DependencyProperty.Register(nameof(Tags), typeof(bool), typeof(SearchTerm), new PropertyMetadata(true));

        public bool Tags
        {
            get { return (bool)GetValue(TagsProperty); }
            set { SetValue(TagsProperty, value); }
        }


        public static readonly DependencyProperty GeographicalOriginsProperty =
            DependencyProperty.Register(nameof(GeographicalOrigins), typeof(bool), typeof(SearchTerm), new PropertyMetadata(true));

        public bool GeographicalOrigins
        {
            get { return (bool)GetValue(GeographicalOriginsProperty); }
            set { SetValue(GeographicalOriginsProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(nameof(Description), typeof(bool), typeof(SearchTerm), new PropertyMetadata(true));

        public bool Description
        {
            get { return (bool)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        #endregion

        #region Search Ranges

        
        public static readonly DependencyProperty IdRangeProperty =
            DependencyProperty.Register(nameof(IdRange), typeof(bool), typeof(SearchTerm), new PropertyMetadata(false));

        public bool IdRange
        {
            get { return (bool)GetValue(IdRangeProperty); }
            set { SetValue(IdRangeProperty, value); }
        }


        public static readonly DependencyProperty AcquiredRangeProperty =
            DependencyProperty.Register(nameof(AcquiredRange), typeof(bool), typeof(SearchTerm), new PropertyMetadata(false));

        public bool AcquiredRange
        {
            get { return (bool)GetValue(AcquiredRangeProperty); }
            set { SetValue(AcquiredRangeProperty, value); }
        }


        public static readonly DependencyProperty CostRangeProperty =
            DependencyProperty.Register(nameof(CostRange), typeof(bool), typeof(SearchTerm), new PropertyMetadata(false));

        public bool CostRange
        {
            get { return (bool)GetValue(CostRangeProperty); }
            set { SetValue(CostRangeProperty, value); }
        }


        #region Ranges


        public static readonly DependencyProperty Id1RangeProperty =
            DependencyProperty.Register(nameof(Id1Range), typeof(string), typeof(SearchTerm), new PropertyMetadata("0"));

        public string Id1Range
        {
            get { return (string)GetValue(Id1RangeProperty); }
            set { SetValue(Id1RangeProperty, value); }
        }


        public static readonly DependencyProperty Id2RangeProperty =
            DependencyProperty.Register(nameof(Id2Range), typeof(string), typeof(SearchTerm), new PropertyMetadata("1000"));

        public string Id2Range
        {
            get { return (string)GetValue(Id2RangeProperty); }
            set { SetValue(Id2RangeProperty, value); }
        }

        
        public static readonly DependencyProperty Acquired1RangeProperty =
            DependencyProperty.Register(nameof(Acquired1Range), typeof(string), typeof(SearchTerm), new PropertyMetadata("1900"));

        public string Acquired1Range
        {
            get { return (string)GetValue(Acquired1RangeProperty); }
            set { SetValue(Acquired1RangeProperty, value); }
        }


        public static readonly DependencyProperty Acquired2RangeProperty =
            DependencyProperty.Register(nameof(Acquired2Range), typeof(string), typeof(SearchTerm), new PropertyMetadata("2050"));

        public string Acquired2Range
        {
            get { return (string)GetValue(Acquired2RangeProperty); }
            set { SetValue(Acquired2RangeProperty, value); }
        }
        

        public static readonly DependencyProperty Cost1RangeProperty =
            DependencyProperty.Register(nameof(Cost1Range), typeof(string), typeof(SearchTerm), new PropertyMetadata("0"));

        public string Cost1Range
        {
            get { return (string)GetValue(Cost1RangeProperty); }
            set { SetValue(Cost1RangeProperty, value); }
        }


        public static readonly DependencyProperty Cost2RangeProperty =
            DependencyProperty.Register(nameof(Cost2Range), typeof(string), typeof(SearchTerm), new PropertyMetadata("10000"));

        public string Cost2Range
        {
            get { return (string)GetValue(Cost2RangeProperty); }
            set { SetValue(Cost2RangeProperty, value); }
        }

        #endregion


        #endregion

        #region Other Options


        public static readonly DependencyProperty CaseSensitiveProperty =
            DependencyProperty.Register(nameof(CaseSensitive), typeof(bool), typeof(SearchTerm), new PropertyMetadata(false));

        public bool CaseSensitive
        {
            get { return (bool)GetValue(CaseSensitiveProperty); }
            set { SetValue(CaseSensitiveProperty, value); }
        }

        
        public static readonly DependencyProperty IsolateProperty =
            DependencyProperty.Register(nameof(Isolate), typeof(bool), typeof(SearchTerm), new PropertyMetadata(false));

        public bool Isolate
        {
            get { return (bool)GetValue(IsolateProperty); }
            set { SetValue(IsolateProperty, value); }
        }


        public static readonly DependencyProperty SearchForEverythingButThisProperty =
            DependencyProperty.Register(nameof(SearchForEverythingButThis), typeof(bool), typeof(SearchTerm), new PropertyMetadata(false));

        public bool SearchForEverythingButThis
        {
            get { return (bool)GetValue(SearchForEverythingButThisProperty); }
            set { SetValue(SearchForEverythingButThisProperty, value); }
        }


        public static readonly DependencyProperty ExcludeFromSearchResultsProperty =
            DependencyProperty.Register(nameof(ExcludeFromSearchResults), typeof(bool), typeof(SearchTerm), new PropertyMetadata(false));

        public bool ExcludeFromSearchResults
        {
            get { return (bool)GetValue(ExcludeFromSearchResultsProperty); }
            set { SetValue(ExcludeFromSearchResultsProperty, value); }
        }


        #endregion


        #region Methods

        public bool Matches(Nativity nativity)
        {
            bool result = false;

            if (SearchForText)
            {
                string text = Text;

                if (Isolate)
                {
                    text = " " + Text + " ";
                }

                if (Id)
                {
                    if (Contains(nativity.Id.ToString(), text, CaseSensitive))
                    {
                        result = true;
                    }
                }

                if (Title)
                {
                    if (Contains(nativity.Title, text, CaseSensitive))
                    {
                        result = true;
                    }
                }

                if (Origin)
                {
                    if (Contains(nativity.Origin, text, CaseSensitive))
                    {
                        result = true;
                    }
                }

                if (Acquired)
                {
                    if (Contains(nativity.Acquired, text, CaseSensitive))
                    {
                        result = true;
                    }
                }

                if (From)
                {
                    if (Contains(nativity.From, text, CaseSensitive))
                    {
                        result = true;
                    }
                }

                if (Cost)
                {
                    if (Contains(nativity.Cost.ToString(), text, CaseSensitive))
                    {
                        result = true;
                    }
                }

                if (Location)
                {
                    if (Contains(nativity.Location, text, CaseSensitive))
                    {
                        result = true;
                    }
                }

                if (Tags)
                {
                    foreach (string t in nativity.Tags)
                    {
                        if (Contains(t, text, CaseSensitive))
                        {
                            result = true;
                        }
                    }
                }

                if (GeographicalOrigins)
                {
                    foreach (string t in nativity.GeographicalOrigins)
                    {
                        if (Contains(t, text, CaseSensitive))
                        {
                            result = true;
                        }
                    }
                }

                if (Description)
                {
                    if (Contains(nativity.Description, text, CaseSensitive))
                    {
                        result = true;
                    }
                }
            }

            else
            {
                result = true;
            }

            if (IdRange)
            {
                if (!(nativity.Id >= GetI(Id1Range) && nativity.Id <= GetI(Id2Range)))
                {
                    result = false;
                }
            }

            if (AcquiredRange)
            {
                if (!(GetI(nativity.Acquired) >= GetI(Acquired1Range) && GetI(nativity.Acquired) <= GetI(Acquired2Range)))
                {
                    result = false;
                }
            }
            
            if (CostRange)
            {
                if (!(nativity.Cost >= GetD(Cost1Range) && nativity.Cost <= GetD(Cost2Range)))
                {
                    result = false;
                }
            }

            return result;
        }

        private int GetI(string text)
        {
            int result = 0;
            try
            {
                result = Convert.ToInt32(text);
            }
            catch { }
            return result;
        }

        private double GetD(string text)
        {
            double result = 0;
            try
            {
                result = Convert.ToDouble(text);
            }
            catch { }
            return result;
        }

        private bool Contains(string input, string text, bool case_sensitive)
        {
            if (case_sensitive)
            {
                return input.Contains(text);
            }

            else
            {
                return input.Contains(text, System.Globalization.CompareOptions.IgnoreCase);
            }
        }

        #endregion
    }

    public class SearchTermCollection : List<SearchTerm>
    {
        public SearchTermCollection()
        { }

        public SearchTermCollection(ListView listView)
        {
            foreach (SearchTerm searchTerm in listView.Items)
            {
                this.Add(searchTerm);
            }
        }
    }

    public class SearchTermCollectionFilter : IFilter
    {
        private IEnumerable<SearchTerm> SearchTerms;
        public SearchTermCollectionFilter(IEnumerable<SearchTerm> searchTerms)
        {
            SearchTerms = searchTerms;
        }

        public bool Filter(object obj)
        {
            return Include((Nativity)obj);
        }

        private bool Include(Nativity nativity)
        {
            bool result = false;

            foreach (SearchTerm searchTerm in SearchTerms)
            {
                if (searchTerm.IsEnabled)
                {
                    if (!searchTerm.SearchForEverythingButThis && !searchTerm.ExcludeFromSearchResults)
                    {
                        if (searchTerm.Matches(nativity))
                        {
                            result = true;
                        }
                    }

                    else if (searchTerm.SearchForEverythingButThis && !searchTerm.ExcludeFromSearchResults)
                    {
                        if (!searchTerm.Matches(nativity))
                        {
                            result = true;
                        }
                    }

                    else if (!searchTerm.SearchForEverythingButThis && searchTerm.ExcludeFromSearchResults)
                    {
                        if (searchTerm.Matches(nativity))
                        {
                            result = false;
                        }
                    }

                    else if (searchTerm.SearchForEverythingButThis && searchTerm.ExcludeFromSearchResults)
                    {
                        bool _result = !searchTerm.Matches(nativity);
                        if (_result)
                        {
                            result = false;
                        }
                    }
                }
            }

            return result;
        }
    }
}
