using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;

namespace Conway_Nativity_Directory
{
    public interface IPreference
    {
        string Title { get; }
        bool Visible { get; }
        bool EffectiveImmediately { get; }
        ObservableCollection<object> Items { get; }
        void Exceptions();
        void Apply();
        void Save(XmlWriter writer);
        void Reset();
        void ResetAll();
        UIElement ShowUI();
        void CloseUI(bool recrusive);
    }
}