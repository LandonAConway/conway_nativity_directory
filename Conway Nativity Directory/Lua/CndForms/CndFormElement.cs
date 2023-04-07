using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLua;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ConwayNativityDirectory.PluginApi;
using NLua.Exceptions;
using System.Windows.Navigation;
using System.Windows.Input;

namespace Conway_Nativity_Directory.Lua_Sandbox
{
    public abstract class CndFormElement
    {
        public CndFormElement() { }

        public CndFormElement(object width, object height)
        {
            Construct();
            SetWidth(width);
            SetHeight(height);
        }

        protected virtual void Construct()
        {
            UIElement.PreviewKeyDown += new KeyEventHandler((object sender, KeyEventArgs e) =>
            {
                object[] values = RunCallback(PreviewKeyDown, this, e.Key.ToString());
                if (values != null && values.Any())
                    e.Handled = (bool)values[0];
            });
            UIElement.KeyDown += new KeyEventHandler((object sender, KeyEventArgs e) =>
            {
                object[] values = RunCallback(OnKeyDown, this, e.Key.ToString());
                if (values != null && values.Any())
                    e.Handled = (bool)values[0];
            });
            UIElement.PreviewKeyUp += new KeyEventHandler((object sender, KeyEventArgs e) =>
            {
                object[] values = RunCallback(PreviewKeyUp, this, e.Key.ToString());
                if (values != null && values.Any())
                    e.Handled = (bool)values[0];
            });
            UIElement.KeyUp += new KeyEventHandler((object sender, KeyEventArgs e) =>
            {
                object[] values = RunCallback(OnKeyUp, this, e.Key.ToString());
                if (values != null && values.Any())
                    e.Handled = (bool)values[0];
            });
        }

        internal abstract UIElement UIElement { get; }

        internal object parent;
        public object GetParent() => parent;

        internal string name;
        public string GetName() => name;

        public double GetX() => Canvas.GetLeft(UIElement);
        public void SetX(double x)
        {
            Canvas.SetLeft(UIElement, x);
            RunCallback(OnPositionChanged, this, x, GetY());
        }

        public double GetY() => Canvas.GetTop(UIElement);
        public void SetY(double y)
        {
            Canvas.SetTop(UIElement, y);
            RunCallback(OnPositionChanged, this, GetX(), y);
        }

        public double GetRight() => Canvas.GetRight(UIElement);

        public void SetRight()
        {
            Canvas.SetRight(UIElement, GetRight());
            RunCallback(OnPositionChanged, this, GetX(), GetY());
        }

        public double GetBottom() => Canvas.GetBottom(UIElement);

        public void SetBottom()
        {
            Canvas.SetBottom(UIElement, GetBottom());
            RunCallback(OnPositionChanged, this, GetX(), GetY());
        }

        public object GetWidth()
        {
            try
            {
                dynamic control = UIElement;
                if (control.Width == double.NaN)
                    return "Auto";
                return control.Width;
            }
            catch { }
            return null;
        }

        public void SetWidth(object value)
        {
            try
            {
                dynamic control = UIElement;
                if (value.ToString() == "Auto")
                    control.Width = double.NaN;
                else
                    control.Width = Convert.ToDouble(value);
                RunCallback(OnSizeChanged, this, value, this.GetHeight(), this.GetActualWidth(), this.GetActualHeight());
            }
            catch { }
        }

        public object GetHeight()
        {
            try
            {
                dynamic control = UIElement;
                if (control.Height == double.NaN)
                    return "Auto";
                return control.Height;
            }
            catch { }
            return null;
        }

        public void SetHeight(object value)
        {
            try
            {
                dynamic control = UIElement;
                if (value.ToString() == "Auto")
                    control.Height = double.NaN;
                else
                    control.Height = Convert.ToDouble(value);
                RunCallback(OnSizeChanged, this, this.GetWidth(), value, this.GetActualWidth(), this.GetActualHeight());
            }
            catch { }
        }

        public double? GetActualWidth()
        {
            try
            {
                dynamic control = UIElement;
                return control.ActualWidth;
            }
            catch { }
            return null;
        }

        public double? GetActualHeight()
        {
            try
            {
                dynamic control = UIElement;
                return control.ActualHeight;
            }
            catch { }
            return null;
        }

        public string GetVerticalAlignment()
        {
            try
            {
                dynamic control = UIElement;
                return control.VerticalAlignment.ToString();
            }
            catch { }
            return null;
        }

        public void SetVerticalAlignment(string value)
        {
            try
            {
                dynamic control = UIElement;
                control.VerticalAlignment = (VerticalAlignment)Enum.Parse(typeof(VerticalAlignment), value);
            }
            catch { }
        }

        public string GetHorizontalAlignment()
        {
            try
            {
                dynamic control = UIElement;
                return control.HorizontalAlignment.ToString();
            }
            catch { }
            return null;
        }

        public void SetHorizontalAlignment(string value)
        {
            try
            {
                dynamic control = UIElement;
                control.HorizontalAlignment = (HorizontalAlignment)Enum.Parse(typeof(HorizontalAlignment), value);
            }
            catch { }
        }

        public int GetRow() => Grid.GetRow(UIElement);

        public void SetRow(int value)
        {
            Grid.SetRow(UIElement, value);
            RunCallback(OnRowChanged, this, value);
        }

        public int GetColumn() => Grid.GetColumn(UIElement);

        public void SetColumn(int value)
        {
            Grid.SetColumn(UIElement, value);
            RunCallback(OnColumnChanged, this, value);
        }

        public int GetRowSpan() => Grid.GetRowSpan(UIElement);

        public void SetRowSpan(int value)
        {
            Grid.SetRowSpan(UIElement, value);
            RunCallback(OnRowSpanChanged, this, value);
        }

        public int GetColumnSpan() => Grid.GetColumnSpan(UIElement);

        public void SetColumnSpan(int value)
        {
            Grid.SetColumnSpan(UIElement, value);
            RunCallback(OnColumnSpanChanged, this, value);
        }

        public string GetBackground()
        {
            try
            {
                dynamic control = UIElement;
                return ((SolidColorBrush)control.Background).Color.ToString();
            }
            catch { }
            return null;
        }

        public void SetBackground(string value)
        {
            try
            {
                dynamic control = UIElement;
                control.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
                RunCallback(OnBackgroundChanged, this, value);
            }
            catch { }
        }

        public string GetBorder()
        {
            try
            {
                dynamic control = UIElement;
                return ((SolidColorBrush)control.BorderBrush).Color.ToString();
            }
            catch { }
            return null;
        }

        public void SetBorder(string value)
        {
            try
            {
                dynamic control = UIElement;
                control.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
                RunCallback(OnBorderChanged, this, value);
            }
            catch { }
        }

        public object[] GetBorderThickness()
        {
            try
            {
                dynamic control = UIElement;
                var left = control.BorderThickness.Left;
                var top = control.BorderThickness.Top;
                var right = control.BorderThickness.Right;
                var bottom = control.BorderThickness.Bottom;
                return new object[] { left, right, top, bottom };
            }
            catch { }
            return null;
        }

        public void SetBorderThickness(double left, double top, double right, double bottom)
        {
            try
            {
                dynamic control = UIElement;
                control.BorderThickness = new Thickness(left, top, right, bottom);
                RunCallback(OnBorderThicknessChanged, this, left, top, right, bottom);
            }
            catch { }
        }

        public object[] GetPadding()
        {
            try
            {
                dynamic control = UIElement;
                var left = control.Padding.Left;
                var top = control.Padding.Top;
                var right = control.Padding.Right;
                var bottom = control.Padding.Bottom;
                return new object[] { left, right, top, bottom };
            }
            catch { }
            return null;
        }

        public void SetPadding(double left, double top, double right, double bottom)
        {
            try
            {
                dynamic control = UIElement;
                control.Padding = new Thickness(left, top, right, bottom);
                RunCallback(OnPaddingChanged, this, left, top, right, bottom);
            }
            catch { }
        }

        public object[] GetMargin()
        {
            try
            {
                dynamic control = UIElement;
                var left = control.Margin.Left;
                var top = control.Margin.Top;
                var right = control.Margin.Right;
                var bottom = control.Margin.Bottom;
                return new object[] { left, right, top, bottom };
            }
            catch { }
            return null;
        }

        public void SetMargin(double left, double top, double right, double bottom)
        {
            try
            {
                dynamic control = UIElement;
                control.Margin = new Thickness(left, top, right, bottom);
                RunCallback(OnMarginChanged, this, left, top, right, bottom);
            }
            catch { }
        }

        public void RemoveFromParent()
        {
            if (this.parent != null)
            {
                if (this.parent is CndForm form)
                    form.SetContent(null);
                else if (this.parent is ContainerElement container)
                    container.RemoveElement(this.name);
                else if (this.parent is ContentElement content)
                    content.SetContent(null);
            }
        }

        public CndForm GetForm() => GetForm(this);

        public static CndForm GetForm(CndFormElement element)
        {
            if (element.parent is CndForm form)
                return form;
            else if (element.parent is ContainerElement container)
                return GetForm(container);
            else if (element.parent is ContentElement content)
                return GetForm(content);
            return null;
        }


        //callbacks
        public LuaFunction OnPositionChanged { get; set; }
        public LuaFunction OnSizeChanged { get; set; }
        public LuaFunction OnRowChanged { get; set; }
        public LuaFunction OnColumnChanged { get; set; }
        public LuaFunction OnRowSpanChanged { get; set; }
        public LuaFunction OnColumnSpanChanged { get; set; }
        public LuaFunction OnBackgroundChanged { get; set; }
        public LuaFunction OnBorderChanged { get; set; }
        public LuaFunction OnBorderThicknessChanged { get; set; }
        public LuaFunction OnPaddingChanged { get; set; }
        public LuaFunction OnMarginChanged { get; set; }
        public LuaFunction PreviewKeyDown { get; set; }
        public LuaFunction OnKeyDown { get; set; }
        public LuaFunction PreviewKeyUp { get; set; }
        public LuaFunction OnKeyUp { get; set; }

        protected object[] RunCallback(LuaFunction callback, params object[] arguments)
        {
            try
            {
                if (callback != null)
                {
                    object[] objects = callback.Call(arguments);
                    return objects;
                }
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

    public class CndFormElementCollection : IEnumerable<CndFormElementCollectionKeyValuePair>
    {
        private List<CndFormElementCollectionKeyValuePair> keyValuePairs = new List<CndFormElementCollectionKeyValuePair>();
        internal CndFormElementCollection() { }

        public CndFormElement this[string key]
        {
            get { return GetValue(key); }
        }

        public CndFormElement GetValue(string key)
        {
            var pair = keyValuePairs.Where(x => x.Key == key).FirstOrDefault();
            if (pair != null)
                return pair.Value;
            return null;
        }

        internal string SetValue(string key, CndFormElement value)
        {
            string result = "None";
            if (key == null)
                return result;
            var current = keyValuePairs.Where(x => x.Key == key).FirstOrDefault();
            if (current != null)
            {
                current.Value.name = null;
                keyValuePairs.Remove(current);
                result = "Removed";
            }
            if (value != null)
            {
                if (result == "Removed")
                    result = "Replaced";
                else
                    result = "Added";
                value.name = key;
                keyValuePairs.Add(new CndFormElementCollectionKeyValuePair(key, value));
            }
            return result;
        }

        public object ToTable()
        {
            LuaScripting.Engine.DoString("cnd._t.table = {}");
            foreach (var item in this)
            {
                LuaScripting.Engine["cnd._t.tableKey"] = item.Key;
                LuaScripting.Engine["cnd._t.tableValue"] = item.Value;
                LuaScripting.Engine.DoString("cnd._t.table[cnd._t.tableKey] = cnd._t.tableValue cnd._t.tableKey = nil cnd._t.tableValue = nil");
            }
            return LuaScripting.Engine.DoString("local table = cnd._t.table cnd._t.table = nil return table");
        }

        public IEnumerator<CndFormElementCollectionKeyValuePair> GetEnumerator() => keyValuePairs.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class CndFormElementCollectionKeyValuePair
    {
        internal CndFormElementCollectionKeyValuePair(string key, CndFormElement value)
        {
            this.key = key;
            this.value = value;
        }

        private string key;
        public string Key
        {
            get { return key; }
        }

        private CndFormElement value;
        public CndFormElement Value
        {
            get { return value; }
        }
    }
}
