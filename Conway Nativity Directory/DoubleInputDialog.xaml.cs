using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for DoubleInputDialog.xaml
    /// </summary>
    public partial class DoubleInputDialog : Window
    {
        public bool FocusOnLoad = true;
        public DoubleInputDialog()
        {
            InitializeComponent();
        }

        public string Response1 {
            get { return ResponseTextBox1.Text; }
            set { ResponseTextBox1.Text = value; }
        }

        public string Response2
        {
            get { return ResponseTextBox2.Text; }
            set { ResponseTextBox2.Text = value; }
        }

        private bool successful = false;
        public bool Successful { get { return successful; } }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            successful = true;
            DialogResult = true;
        }

        private void ResponseTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(null, null);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (FocusOnLoad == true)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Input,
                    new Action(delegate () {
                        ResponseTextBox1.Focus();
                        ResponseTextBox1.CaretIndex = ResponseTextBox1.Text.Count();
                        Keyboard.Focus(ResponseTextBox1);
                    }));
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!Successful)
            {
                DialogResult = false;
            }
        }

        private void CommandBinding_Close(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
