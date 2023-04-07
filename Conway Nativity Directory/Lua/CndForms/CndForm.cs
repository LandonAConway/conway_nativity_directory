using ConwayNativityDirectory.PluginApi;
using ConwayNativityDirectory.PluginApi.Primitives;
using NLua;
using NLua.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Conway_Nativity_Directory.Lua_Sandbox
{
    public class CndForm
    {
        //Test it:
        //lua w = CndForm("Test", 500, 500) w:SetTopmost(true) w:AddElement("label1", LabelElement("Test", 10, 10)) w:ShowDialog()

        public CndForm()
        {
            Construct();
        }

        public CndForm(object width, object height)
        {
            Construct();
            SetWidth(width);
            SetHeight(height);
        }

        public CndForm(string title)
        {
            Construct();
            window.Title = title;
        }

        public CndForm(string title, object width, object height)
        {
            Construct();
            window.Title = title;
            SetWidth(width);
            SetHeight(height);
        }

        private void Construct()
        {
            window = new Window();
            canvas = new Canvas();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Closed += new EventHandler((object sender, EventArgs e) => { isClosed = true; RunCallback(OnClosed, this); });
            window.ContentRendered += new EventHandler((object sender, EventArgs e) => { RunCallback(OnRendered, this); });
        }

        private void RecreateWindow()
        {
            var content = window.Content;
            window.Content = null;
            Window newWindow = new Window();
            newWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            newWindow.Content = content;
            newWindow.Width = window.Width;
            newWindow.Height = window.Height;
            newWindow.ResizeMode = window.ResizeMode;
            newWindow.Background = window.Background;
            newWindow.Title = window.Title;
            newWindow.Topmost = window.Topmost;
            window = newWindow;
            window.Closed += new EventHandler((object sender, EventArgs e) => { isClosed = true; RunCallback(OnClosed, this); });
            window.ContentRendered += new EventHandler((object sender, EventArgs e) => { RunCallback(OnRendered, this); });
        }

        private Window window;
        private Canvas canvas;

        private bool isClosed = false;
        public void Show()
        {
            if (isClosed)
                RecreateWindow();
            isClosed = false;
            window.Show();
        }

        public bool? ShowDialog()
        {
            if (isClosed)
                RecreateWindow();
            isClosed = false;
            return window.ShowDialog();
        }

        public void Close() => window.Close();
        public void Hide() => window.Hide();

        public void SetWidth(object value)
        {
            if (value.ToString() == "Auto")
                window.Width = double.NaN;
            else
                window.Width = Convert.ToDouble(value);
            RunCallback(OnSizeChanged, this, value, this.GetHeight(), this.GetActualWidth(), this.GetActualHeight());
        }

        public double GetWidth() => window.Width;
        public double GetActualWidth() => window.ActualWidth;

        public void SetHeight(object value)
        {
            if (value.ToString() == "Auto")
                window.Height = double.NaN;
            else
                window.Height = Convert.ToDouble(value);
            RunCallback(OnSizeChanged, this, this.GetHeight(), value, this.GetActualWidth(), this.GetActualHeight());
        }

        public double GetHeight() => window.Height;
        public double GetActualHeight() => window.ActualHeight;

        public void SetResizeMode(string resizeMode) => window.ResizeMode = (ResizeMode)Enum.Parse(typeof(ResizeMode), resizeMode);
        public string GetResizeMode() => window.ResizeMode.ToString();

        public void SetBackground(string value) => canvas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
        public string GetBackground() => (canvas.Background as SolidColorBrush)?.Color.ToString();

        public void SetTitle(string value)
        {
            window.Title = value;
            RunCallback(OnTitleChanged, this, value);
        }
        public string GetTitle() => window.Title.ToString();

        public void SetTopmost(bool value) => window.Topmost = value;
        public bool GetTopmost() => window.Topmost;


        //Content
        private CndFormElement content;

        public CndFormElement GetContent => content;
        public void SetContent(CndFormElement content)
        {
            if (content == null)
            {
                window.Content = null;
                this.content.parent = null;
                this.content = null;
            }

            else
            {
                window.Content = content.UIElement;
                content.RemoveFromParent();
                content.parent = this;
                this.content = content;
            }
        }

        //Callbacks

        public LuaFunction OnRendered { get; set; }
        public LuaFunction OnClosed { get; set; }
        public LuaFunction OnSizeChanged { get; set; }
        public LuaFunction OnTitleChanged { get; set; }

        protected object[] RunCallback(LuaFunction callback, params object[] arguments)
        {
            try
            {
                if (callback != null)
                    return callback.Call(arguments);
            }

            catch (Exception ex)
            {
                LuaScriptException lse = ex as LuaScriptException;
                if (lse != null && lse.InnerException != null)
                    MessageBox.Show("Failed to run script: " + lse.InnerException.Message);
                else
                    MessageBox.Show("Failed to run script: " + ex.Message);
            }
            return null;
        }
    }
}
