using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Conway_Nativity_Directory
{
    public partial class WaitForm: Form
    {
        public WaitForm(string text)
        {
            InitializeComponent();
            label1.Text = text;
        }

        public string LabelText
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }
    }
}
