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
    /// Interaction logic for Tag.xaml
    /// </summary>
    public partial class Tag : UserControl
    {


        #region Constructor

        public Tag()
        {
            InitializeComponent();
        }

        #endregion


        #region Routed Events

        #region RemoveButtonClick
        
        public static readonly RoutedEvent RemoveButtonClickEvent = EventManager.RegisterRoutedEvent(
            "RemoveButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Tag));
        
        public event RoutedEventHandler RemoveButtonClick
        {
            add { AddHandler(RemoveButtonClickEvent, value); }
            remove { RemoveHandler(RemoveButtonClickEvent, value); }
        }
        
        void RaiseRemoveButtonClickEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(Conway_Nativity_Directory.Tag.RemoveButtonClickEvent);
            RaiseEvent(newEventArgs);
        }

        #endregion

        #endregion


        #region Dependency Properties

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Tag));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        public static readonly DependencyProperty BrushProperty =
            DependencyProperty.Register("Brush", typeof(Brush), typeof(Tag), new PropertyMetadata(Brushes.LightGray));

        public Brush Brush
        {
            get { return (Brush)GetValue(BrushProperty); }
            set { SetValue(BrushProperty, value); }
        }


        public static readonly DependencyProperty MouseOverBrushProperty =
            DependencyProperty.Register("MouseOverBrush", typeof(Brush), typeof(Tag), new PropertyMetadata(Brushes.LightGray));

        public Brush MouseOverBrush
        {
            get { return (Brush)GetValue(MouseOverBrushProperty); }
            set { SetValue(MouseOverBrushProperty, value); }
        }


        public new static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(Tag), new PropertyMetadata(Brushes.Black));

        public new Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }


        public new static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(Tag), new PropertyMetadata(new Thickness(0)));

        public new Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }


        public new static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(Brush), typeof(Tag), new PropertyMetadata(Brushes.Black));

        public new Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }


        public static readonly DependencyProperty ButtonsBrushProperty =
            DependencyProperty.Register("ButtonsBrush", typeof(Brush), typeof(Tag), new PropertyMetadata(Brushes.Black));

        public Brush ButtonsBrush
        {
            get { return (Brush)GetValue(ButtonsBrushProperty); }
            set { SetValue(ButtonsBrushProperty, value); }
        }


        public static readonly DependencyProperty ButtonsMouseOverBrushProperty =
            DependencyProperty.Register("ButtonsMouseOverBrush", typeof(Brush), typeof(Tag), new PropertyMetadata(Brushes.White));

        public Brush ButtonsMouseOverBrush
        {
            get { return (Brush)GetValue(ButtonsMouseOverBrushProperty); }
            set { SetValue(ButtonsMouseOverBrushProperty, value); }
        }


        public static readonly DependencyProperty RemoveButtonVisibilityProperty =
            DependencyProperty.Register("RemoveButtonVisibility", typeof(Visibility), typeof(Tag), new PropertyMetadata(Visibility.Visible));

        public Visibility RemoveButtonVisibility
        {
            get { return (Visibility)GetValue(RemoveButtonVisibilityProperty); }
            set { SetValue(RemoveButtonVisibilityProperty, value); }
        }


        #region Read-only


        private static readonly DependencyPropertyKey IsPressedPropertyKey
        = DependencyProperty.RegisterReadOnly(
            nameof(IsPressed), typeof(bool), typeof(Tag),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));

        public static readonly DependencyProperty IsPressedProperty
            = IsPressedPropertyKey.DependencyProperty;

        public bool IsPressed
        {
            get { return (bool)GetValue(IsPressedProperty); }
            protected set { SetValue(IsPressedPropertyKey, value); }
        }


        #endregion


        #endregion


        #region Events

        private void RemoveButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RaiseRemoveButtonClickEvent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.IsPressed = true;
        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.IsPressed = false;
        }


        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed ||
                Mouse.MiddleButton == MouseButtonState.Pressed ||
                Mouse.RightButton == MouseButtonState.Pressed)
            {
                this.IsPressed = true;
            }

            else
            {
                this.IsPressed = false;
            }
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            this.IsPressed = false;
        }


        #endregion


    }

    public class DivideConverter : IValueConverter
    {
        private double divisor = 1;
        public double Divisor {
            get { return divisor; }
            set { divisor = value; }
        }

        public object Convert(object value, Type targetType, object target, System.Globalization.CultureInfo culture)
        {
            if (value is int)
                return System.Convert.ToInt32((int)value / Divisor);
            else
                return (double)value / Divisor;
        }

        public object ConvertBack(object value, Type targetType, object target, System.Globalization.CultureInfo culture)
        {
            if (value is int)
                return System.Convert.ToInt32((int)value * Divisor);
            else
                return (double)value * Divisor;
        }
    }
}
