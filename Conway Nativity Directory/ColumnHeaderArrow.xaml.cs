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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for ColumnHeaderArrow.xaml
    /// </summary>
    public partial class ColumnHeaderArrow : UserControl
    {


        #region Constructor

        public ColumnHeaderArrow()
        {
            InitializeComponent();
        }

        #endregion

        
        #region DependencyProperties


        public static readonly DependencyProperty SortingModeProperty =
            DependencyProperty.Register("SortingMode", typeof(SortingMode), typeof(ColumnHeaderArrow), new PropertyMetadata(SortingMode.Ascending));

        /// <summary>
        /// Gets and sets <see cref="ColumnHeaderArrow.SortingMode"/>.
        /// </summary>
        public SortingMode SortingMode
        {
            get { return (SortingMode)GetValue(SortingModeProperty); }
            set
            {
                if (value == SortingMode.Ascending)
                    path.Data = Geometry.Parse("M0,1 L1,0 L2,1");
                else if (value == SortingMode.Descending)
                    path.Data = Geometry.Parse("M0,0 L1,1 L2,0");

                SetValue(SortingModeProperty, value);
            }
        }

        #endregion


        #region Private Methods

        private void SetBindings()
        {
        }

        #endregion


    }
}
