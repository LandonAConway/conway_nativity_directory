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
using ConwayNativityDirectory.PluginApi;

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for ProgressBarDialog.xaml
    /// </summary>
    public partial class ProgressBarWindow : Window
    {
        public ProgressBarWindow(ProgressBarModal modal)
        {
            InitializeComponent();
            Modal = modal;
            updatePbDelegate = new UpdatePropertyDelegate(this.Progress.SetValue);
            updateTitleDelegate = new UpdatePropertyDelegate(this.SetValue);
            updateInfoDelegate = new UpdatePropertyDelegate(this.InfoLabel.SetValue);
            this.Closing += ProgressBarWindow_Closing;
            this.ContentRendered += ProgressBarWindow_ContentRendered;
            this.Progress.Maximum = modal.Max;
            UpdateValue();
            UpdateTitle();
            UpdateInfo();
        }

        private void ProgressBarWindow_ContentRendered(object sender, EventArgs e)
        {
            Start();
        }

        ProgressBarModal Modal { get; set; }

        public bool working = false;
        public void Start()
        {
            working = true;
            Modal.Process(Modal);
            working = false;
            this.Close();
        }

        UpdatePropertyDelegate updatePbDelegate;
        public void UpdateValue()
        {
            Dispatcher.Invoke(updatePbDelegate,
                System.Windows.Threading.DispatcherPriority.Background,
                new object[] { ProgressBar.ValueProperty, Modal.Value });
        }

        UpdatePropertyDelegate updateTitleDelegate;
        public void UpdateTitle()
        {
            Dispatcher.Invoke(updateTitleDelegate,
                System.Windows.Threading.DispatcherPriority.Background,
                new object[] { Window.TitleProperty, Modal.Title });
        }

        UpdatePropertyDelegate updateInfoDelegate;
        public void UpdateInfo()
        {
            Dispatcher.Invoke(updateInfoDelegate,
                System.Windows.Threading.DispatcherPriority.Background,
                new object[] { TextBlock.TextProperty, Modal.Info });

            Dispatcher.Invoke(new UpdatePropertyDelegate(this.SetValue),
                System.Windows.Threading.DispatcherPriority.Background,
                new object[] { Window.HeightProperty, 0.0 });

            Dispatcher.Invoke(new UpdatePropertyDelegate(this.SetValue),
                System.Windows.Threading.DispatcherPriority.Background,
                new object[] { Window.HeightProperty, double.NaN });
        }

        public bool closed = false;
        private void ProgressBarWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.working)
                e.Cancel = true;
        }
    }

    public delegate void UpdatePropertyDelegate(System.Windows.DependencyProperty dp, Object value);

    public class ProgressBarModal : ProgressBarModalRef
    {
        string title = "";
        public override string Title 
        {
            get { return title; }
            set
            {
                title = value;
                if (window != null)
                    window.UpdateTitle();
            }
        }

        string info = "";
        public override string Info
        {
            get { return info; }
            set
            {
                info = value;
                if (window != null)
                    window.UpdateInfo();
            }
        }

        double _value = 0;
        public override double Value 
        { 
            get { return _value; } 
            set
            {
                _value = value;
                if (window != null)
                    window.UpdateValue();
            }
        }

        double max = 100;
        public override double Max
        {
            get { return max; }
            set
            {
                max = value;
                if (window != null)
                    window.Progress.Maximum = value;
            }
        }

        public override ProgressBarModalProcess Process { get; set; }

        ProgressBarWindow window;

        public override void Show()
        {
            if (window == null)
            {
                ProgressBarWindow w = new ProgressBarWindow(this);
                window = w;
                w.ShowDialog();
                Title = "";
                Max = 100;
                Value = 0;
                window = null;
            }
        }

        public override void Increment()
        {
            Value = Value + 1;
        }
    }

    ///// <summary>
    ///// Interaction logic for Window1.xaml
    ///// </summary>
    //public partial class Window1 : Window
    //{
    //	public Window1()
    //	{
    //		InitializeComponent();
    //	}

    //	private void Button1_Click(object sender, RoutedEventArgs e)
    //	{
    //		Process();
    //	}


    //	//********************************
    //	//  WPF ProgressBar Demo
    //	//  Written by VBRocks, 7/28/09
    //	//********************************


    //	//Create a Delegate that matches the Signature of the ProgressBar's SetValue method
    //	private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);


    //	private void Process()
    //	{
    //		//Configure the ProgressBar
    //		ProgressBar1.Minimum = 0;
    //		ProgressBar1.Maximum = short.MaxValue;
    //		ProgressBar1.Value = 0;

    //		//Stores the value of the ProgressBar
    //		double value = 0;

    //		//Create a new instance of our ProgressBar Delegate that points
    //		//  to the ProgressBar's SetValue method.
    //		UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(ProgressBar1.SetValue);

    //		//Tight Loop:  Loop until the ProgressBar.Value reaches the max
    //		do
    //		{
    //			value += 1;

    //            /*Update the Value of the ProgressBar:
    //	          1)  Pass the "updatePbDelegate" delegate that points to the ProgressBar1.SetValue method
    //		      2)  Set the DispatcherPriority to "Background"
    //			  3)  Pass an Object() Array containing the property to update (ProgressBar.ValueProperty) and the new value */
    //			Dispatcher.Invoke(updatePbDelegate, 
    //				System.Windows.Threading.DispatcherPriority.Background, 
    //				new object[] { ProgressBar.ValueProperty, value });

    //		}
    //		while (ProgressBar1.Value != ProgressBar1.Maximum);

    //	}
    //}
}
