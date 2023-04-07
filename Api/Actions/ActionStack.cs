using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ConwayNativityDirectory.PluginApi
{
    public interface IAction
    {
        /// <summary>
        /// Undoes an action.
        /// </summary>
        void Redo();

        /// <summary>
        /// Redoes an action.
        /// </summary>
        void Undo();

        /// <summary>
        /// Gets the description of the action.
        /// </summary>
        string Description { get; }
    }

    /// <summary>
    /// Holds a collection of <see cref="IAction"/> interfaces that can be selected.
    /// </summary>
    public class ActionStack : List<IAction>
    {
        /// <summary>
        /// Occurs when a <see cref="IAction"/> is added to an <see cref="ActionStack"/>.
        /// </summary>
        public event ActionAddedEventHandler ActionAdded;

        /// <summary>
        /// Occurs when <see cref="IAction.Undo"/> is called in an <see cref="ActionStack"/>.
        /// </summary>
        public event ActionUndoneEventHandler ActionUndone;

        /// <summary>
        /// Occurs when <see cref="IAction.Redo"/> is called in an <see cref="ActionStack"/>.
        /// </summary>
        public event ActionRedoneEventHandler ActionRedone;

        /// <summary>
        /// Occurs after the value of <see cref="SelectedIndex"/> has changed.
        /// </summary>
        public event EventHandler SelectedIndexChanged;

        /// <summary>
        /// Gets the index of the next action that would be undone.
        /// </summary>
        public int SelectedIndex { get { return current; } }
        
        private int _current;
        private int current {
            get { return _current; }
            set
            {
                _current = Math.Max(-1, Math.Min(this.Count, value));
                
                EventHandler handler = SelectedIndexChanged;
                if (handler != null)
                {
                    handler(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum amount of actions.
        /// </summary>
        public int Max { get; set; }

        /// <summary>
        /// Adds an object to the end of the <see cref="List{T}"/>
        /// </summary>
        new public void Add(IAction item)
        {
            ActionAddedEventHandler handler = ActionAdded;
            if (handler != null)
            {
                ActionAddedEventArgs args = new ActionAddedEventArgs(item);
                handler(this, args);
                if (args.Cancel) { return; }
            }
            try { this.RemoveRange(current + 1, this.Count - (current + 1)); } catch { }
            current = this.Count;
            base.Add(item);
            
            if (this.Max > 0)
            {
                if (this.Count > this.Max)
                {
                    this.RemoveAt(0);
                    current -= 1;
                }
            }
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the <see cref="List{T}"/>
        /// </summary>
        /// <exception cref="System.ArgumentNullException"></exception>
        new public void AddRange(IEnumerable<IAction> collection)
        {
            ActionAddedEventHandler handler = ActionAdded;
            if (handler != null)
            {
                ActionAddedEventArgs args = new ActionAddedEventArgs(collection.Last());
                handler(this, args);
                if (args.Cancel) { return; }
            }
            
            try { this.RemoveRange(current + 1, this.Count - (current + 1)); } catch { }
            current = (this.Count - 1) + collection.Count();
            base.AddRange(collection);

            if (this.Max > 0)
            {
                if (this.Count > this.Max)
                {
                    this.RemoveRange(0,collection.Count());
                    current -= collection.Count();
                }
            }
        }

        /// <summary>
        /// Sets the index of the next action that would be undone.
        /// </summary>
        /// <param name="id"></param>
        public void SetSelectedIndex(int id)
        {
            if (id > (this.Count -1))
                throw new ArgumentOutOfRangeException(nameof(id), "Index was out of range. Must be non-negative and less than the size of the collection.");
            current = id;
        }

        /// <summary>
        /// Calls the <see cref="IAction.Undo"/> of the selected <see cref="IAction"/>.
        /// </summary>
        /// <returns>Returns false is no more actions can be undone</returns>
        public bool Undo()
        {
            if (current >= 0)
            {
                IAction action = this.ElementAt(current);

                ActionUndoneEventHandler handler = ActionUndone;
                if (handler != null)
                {
                    ActionUndoneEventArgs args = new ActionUndoneEventArgs(action);
                    handler(this, args);
                    if (args.Cancel) { return false; }
                }

                action.Undo();
                current = current - 1;
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Calls the <see cref="IAction.Redo"/> of the next <see cref="IAction"/>.
        /// </summary>
        /// <returns>Returns false is no more actions can be redone</returns>
        public bool Redo()
        {
            if (current + 1 < this.Count)
            {
                IAction action = this.ElementAt(current + 1);

                ActionRedoneEventHandler handler = ActionRedone;
                if (handler != null)
                {
                    ActionRedoneEventArgs args = new ActionRedoneEventArgs(action);
                    handler(this, args);
                    if (args.Cancel) { return false; }
                }

                action.Redo();
                current = current + 1;
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Represents a method that commonly handles an <see cref="IAction"/> bieng added to a <see cref="ActionStack"/>.
    /// </summary>
    public delegate void ActionAddedEventHandler(object sender, ActionAddedEventArgs e);

    /// <summary>
    /// Contains arguments relevent to an <see cref="IAction"/>.
    /// </summary>
    public class ActionAddedEventArgs : EventArgs
    {
        private IAction _action;

        public ActionAddedEventArgs(IAction action)
        {
            this._action = action;
        }

        public bool Cancel { get; set; }

        public IAction Action
        {
            get
            {
                return _action;
            }
        }
    }

    /// <summary>
    /// Represents a method that commonly handles <see cref="ActionStack.Undo"/> successfuly bieng called from an <see cref="ActionStack"/>.
    /// </summary>
    public delegate void ActionUndoneEventHandler(object sender, ActionUndoneEventArgs e);

    /// <summary>
    /// Contains arguments relevent to an <see cref="IAction"/>.
    /// </summary>
    public class ActionUndoneEventArgs : EventArgs
    {
        private IAction _action;

        public ActionUndoneEventArgs(IAction action)
        {
            this._action = action;
        }

        public bool Cancel { get; set; }

        public IAction Action
        {
            get
            {
                return _action;
            }
        }
    }

    /// <summary>
    /// Represents a method that commonly handles <see cref="ActionStack.Redo"/> successfuly bieng called from an <see cref="ActionStack"/>.
    /// </summary>
    public delegate void ActionRedoneEventHandler(object sender, ActionRedoneEventArgs e);

    /// <summary>
    /// Contains arguments relevent to an <see cref="IAction"/>.
    /// </summary>
    public class ActionRedoneEventArgs : EventArgs
    {
        private IAction _action;

        public ActionRedoneEventArgs(IAction action)
        {
            this._action = action;
        }

        public bool Cancel { get; set; }

        public IAction Action
        {
            get
            {
                return _action;
            }
        }
    }
}
