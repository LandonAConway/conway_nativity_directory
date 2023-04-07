using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for CustomRichTextBox.xaml
    /// </summary>
    public partial class CustomRichTextBox : RichTextBox
    {
        public CustomRichTextBox()
        {
            InitializeComponent();
        }

        public string Text
        {
            get
            {
                return new TextRange(Document.ContentStart, Document.ContentEnd).Text;
            }

            set
            {
                Document.Blocks.Clear();
                Document.Blocks.Add(new Paragraph(new Run(value)));
                
                if (HighlightIsOn)
                {
                    List<HighlightKeywordSet> sets = new List<HighlightKeywordSet>(HighlightKeywordSets);
                    HighlightKeywordSets.Clear();
                    HighlightKeywords(sets);
                }
            }
        }

        private bool HighlightIsOn { get; set; }
        private List<HighlightKeywordSet> HighlightKeywordSets = new List<HighlightKeywordSet>();

        public void HighlightKeyword(Brush brush, string keyword, CompareOptions compareOption)
        {
            HighlightIsOn = true;
            HighlightKeywordSets.Add(new HighlightKeywordSet(brush, keyword, compareOption));

            IEnumerable<TextRange> wordRanges = GetAllWordRanges(Document);
            foreach (TextRange wordRange in wordRanges)
            {
                if (StringContains(wordRange.Text, keyword, compareOption))
                {
                    wordRange.ApplyPropertyValue(TextElement.BackgroundProperty, brush);
                }
            }
        }

        public void HighlightKeywords(List<HighlightKeywordSet> highlightKeywordSets)
        {
            foreach (HighlightKeywordSet set in new List<HighlightKeywordSet>(highlightKeywordSets))
            {
                HighlightKeyword(set.Brush, set.Keyword, set.CompareOption);
            }
        }

        public void ClearHighlights()
        {
            HighlightIsOn = false;
            HighlightKeywordSets.Clear();

            string oldText = Text;
            Document.Blocks.Clear();
            Document.Blocks.Add(new Paragraph(new Run(oldText)));
        }

        public static IEnumerable<TextRange> GetAllWordRanges(FlowDocument document)
        {
            string pattern = @"[^\W\d](\w|[-']{1,3}(?=\w))*";
            TextPointer pointer = document.ContentStart;
            while (pointer != null)
            {
                if (pointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string textRun = pointer.GetTextInRun(LogicalDirection.Forward);
                    MatchCollection matches = Regex.Matches(textRun, pattern);
                    foreach (Match match in matches)
                    {
                        int startIndex = match.Index;
                        int length = match.Length;
                        TextPointer start = pointer.GetPositionAtOffset(startIndex);
                        TextPointer end = start.GetPositionAtOffset(length);
                        yield return new TextRange(start, end);
                    }
                }

                pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
            }
        }

        private static bool StringContains(string source, string content, CompareOptions compareOption)
        {
            bool result = false;

            CultureInfo culture = new CultureInfo("es-ES");
            if (culture.CompareInfo.IndexOf(source, content, compareOption) >= 0)
            {
                result = true;
            }

            return result;
        }
    }

    public class HighlightKeywordSet
    {
        public HighlightKeywordSet(Brush brush, string keyword, CompareOptions compareOption)
        {
            this.brush = brush;
            this.keyword = keyword;
            this.compareOption = compareOption;
        }

        private Brush brush;
        public Brush Brush { get { return brush; } }

        private string keyword;
        public string Keyword { get { return keyword; } }

        private CompareOptions compareOption;
        public CompareOptions CompareOption { get { return compareOption; } }
    }
}
