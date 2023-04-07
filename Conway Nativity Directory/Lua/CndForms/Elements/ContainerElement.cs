using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLua;
using System.Windows;
using System.Windows.Controls;

namespace Conway_Nativity_Directory.Lua_Sandbox
{
    public abstract class ContainerElement : CndFormElement
    {
        //Elements
        private CndFormElementCollection elements = new CndFormElementCollection();
        public virtual CndFormElementCollection GetElements() => elements;
        protected CndFormElementCollection Elements => GetElements();

        public virtual void ClearElements()
        {
            foreach (var element in Elements.ToArray())
                RemoveElement(element.Key);
        }

        protected virtual void OnAddElement(string name, CndFormElement added, string result) { }
        protected virtual void OnRemoveElement(string name, CndFormElement removed, string result) { }

        public void AddElement(string name, CndFormElement element)
        {
            element.RemoveFromParent();
            element.parent = this;
            string result = elements.SetValue(name, element);
            OnAddElement(name, element, result);
        }

        public CndFormElement RemoveElement(string name)
        {
            string result = "None";
            var element = elements.GetValue(name);
            if (element != null)
            {
                element.parent = null;
                result = elements.SetValue(name, null);
            }
            OnRemoveElement(name, element, result);
            return element;
        }
    }
}
