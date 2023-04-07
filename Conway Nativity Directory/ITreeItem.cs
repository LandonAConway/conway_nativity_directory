using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Conway_Nativity_Directory
{
    public interface ITreeItem
    {
        bool IsSelected { get; set; }
        bool IsExpanded { get; set; }

        ObservableCollection<object> Items { get; }
    }

    public class TreeItem : FrameworkElement, ITreeItem
    {
        #region Dependency Properties

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(OptimizationPreference),
                new PropertyMetadata(false));

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }


        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(OptimizationPreference),
                new PropertyMetadata(false));

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        #endregion


        #region Public Properties

        public ObservableCollection<object> Items { get { return items; } }

        #endregion


        #region Private Properties

        #region For get-only properties

        private ObservableCollection<object> items = new ObservableCollection<object>();

        #endregion

        #endregion
    }
}
