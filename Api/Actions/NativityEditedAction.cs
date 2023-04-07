using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ConwayNativityDirectory.PluginApi.Primitives;

namespace ConwayNativityDirectory.PluginApi
{
    public class NativityEditedAction : IAction
    {

        private NativityPropertyChangeInfo[] _nativities;
        public NativityPropertyChangeInfo[] Nativities
        {
            get { return _nativities; }
        }

        private NativityProperty _property;
        public NativityProperty Property { get { return _property; } }

        public NativityEditedAction(IEnumerable<NativityPropertyChangeInfo> nativities, NativityProperty property)
        {
            _nativities = nativities.ToArray();
            _property = property;

            switch (property)
            {
                case NativityProperty.Id:
                    Description = "Change [" + nativities.Count().ToString() +
                        "] " + _nativityP() + " " + _propertyP("Id");
                    break;
                case NativityProperty.Title:
                    Description = "Change [" + nativities.Count().ToString() +
                        "] " + _nativityP() + " " + _propertyP("Title");
                    break;
                case NativityProperty.Origin:
                    Description = "Change [" + nativities.Count().ToString() +
                        "] " + _nativityP() + " " + _propertyP("Origin");
                    break;
                case NativityProperty.Acquired:
                    Description = "Change [" + nativities.Count().ToString() +
                        "] " + _nativityP() + " " + _propertyP("Acquired");
                    break;
                case NativityProperty.From:
                    Description = "Change [" + nativities.Count().ToString() +
                        "] " + _nativityP() + " " + _propertyP("From");
                    break;
                case NativityProperty.Cost:
                    Description = "Change [" + nativities.Count().ToString() +
                        "] " + _nativityP() + " " + _propertyP("Cost");
                    break;
                case NativityProperty.Location:
                    Description = "Change [" + nativities.Count().ToString() +
                        "] " + _nativityP() + " " + _propertyP("Location");
                    break;
                case NativityProperty.Tags:
                    Description = "Change [" + nativities.Count().ToString() +
                        "] " + _nativityP() + " Tags";
                    break;
                case NativityProperty.GeographicalOrigins:
                    Description = "Change [" + nativities.Count().ToString() +
                        "] " + _nativityP() + " Geographical Origins";
                    break;
                case NativityProperty.Description:
                    Description = "Change [" + nativities.Count().ToString() +
                        "] " + _nativityP() + " " + _propertyP("Description");
                    break;
            }
        }

        private string _nativityP()
        {
            if (Nativities.Count() < 1 || Nativities.Count() > 1)
                return "Nativities";
            else
                return "Nativity's";
        }

        private string _propertyP(string p)
        {
            if (Nativities.Count() < 1 || Nativities.Count() > 1)
                return p + "'s";
            else
                return p;
        }

        public string Description { get; private set; }

        public void Redo() 
        {
            foreach (var nativity in _nativities)
            {
                switch (Property)
                {
                    case NativityProperty.Id:
                        nativity.Target.Id = Convert.ToInt32(nativity.NewValue);
                        break;
                    case NativityProperty.Title:
                        nativity.Target.Title = nativity.NewValue.ToString();
                        break;
                    case NativityProperty.Origin:
                        nativity.Target.Origin = nativity.NewValue.ToString();
                        break;
                    case NativityProperty.Acquired:
                        nativity.Target.Acquired = nativity.NewValue.ToString();
                        break;
                    case NativityProperty.From:
                        nativity.Target.Acquired = nativity.NewValue.ToString();
                        break;
                    case NativityProperty.Cost:
                        nativity.Target.Cost = Convert.ToDouble(nativity.NewValue);
                        break;
                    case NativityProperty.Location:
                        nativity.Target.Location = nativity.NewValue.ToString();
                        break;
                    case NativityProperty.Tags:
                        nativity.Target.Tags = nativity.NewValue as ObservableCollection<string>;
                        break;
                    case NativityProperty.GeographicalOrigins:
                        nativity.Target.GeographicalOrigins = nativity.NewValue as ObservableCollection<string>;
                        break;
                    case NativityProperty.Description:
                        nativity.Target.Description = nativity.NewValue.ToString();
                        break;
                }
            }
        }

        public void Undo() 
        {
            foreach (var nativity in _nativities)
            {
                switch (Property)
                {
                    case NativityProperty.Id:
                        nativity.Target.Id = Convert.ToInt32(nativity.OldValue);
                        break;
                    case NativityProperty.Title:
                        nativity.Target.Title = nativity.OldValue.ToString();
                        break;
                    case NativityProperty.Origin:
                        nativity.Target.Origin = nativity.OldValue.ToString();
                        break;
                    case NativityProperty.Acquired:
                        nativity.Target.Acquired = nativity.OldValue.ToString();
                        break;
                    case NativityProperty.From:
                        nativity.Target.Acquired = nativity.OldValue.ToString();
                        break;
                    case NativityProperty.Cost:
                        nativity.Target.Cost = Convert.ToDouble(nativity.OldValue);
                        break;
                    case NativityProperty.Location:
                        nativity.Target.Location = nativity.OldValue.ToString();
                        break;
                    case NativityProperty.Tags:
                        nativity.Target.Tags = nativity.OldValue as ObservableCollection<string>;
                        break;
                    case NativityProperty.GeographicalOrigins:
                        nativity.Target.GeographicalOrigins = nativity.OldValue as ObservableCollection<string>;
                        break;
                    case NativityProperty.Description:
                        nativity.Target.Description = nativity.OldValue.ToString();
                        break;
                }
            }
        }
    }

    public class NativityPropertyChangeInfo
    {
        public object OldValue { get; set; }
        public object NewValue { get; set; }

        NativityBase target;
        public NativityBase Target { get { return target; } }

        public NativityPropertyChangeInfo(NativityBase target)
        {
            this.target = target;
        }

        public NativityPropertyChangeInfo(NativityBase target, object oldValue, object newValue)
        {
            this.target = target;
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
    }

    public class NativitiesEditedAction : IAction
    {
        public string Description { get; set; }

        List<NativityEditedInfo> nativities = new List<NativityEditedInfo>();
        public List<NativityEditedInfo> Nativities { get { return nativities; } }

        public void Redo()
        {
            foreach (var info in nativities)
            {
                var nativity = info.Target;
                nativity.Id = (int)info.Id.NewValue;
                nativity.Title = (string)info.Title.NewValue;
                nativity.Origin = (string)info.Origin.NewValue;
                nativity.Acquired = (string)info.Acquired.NewValue;
                nativity.From = (string)info.From.NewValue;
                nativity.Cost = (double)info.Cost.NewValue;
                nativity.Location = (string)info.Location.NewValue;
                nativity.Tags = (ObservableCollection<string>)info.Tags.NewValue;
                nativity.GeographicalOrigins = (ObservableCollection<string>)info.GeographicalOrigins.NewValue;
            }
        }

        public void Undo()
        {
            foreach (var info in nativities)
            {
                var nativity = info.Target;
                nativity.Id = (int)info.Id.OldValue;
                nativity.Title = (string)info.Title.OldValue;
                nativity.Origin = (string)info.Origin.OldValue;
                nativity.Acquired = (string)info.Acquired.OldValue;
                nativity.From = (string)info.From.OldValue;
                nativity.Cost = (double)info.Cost.OldValue;
                nativity.Location = (string)info.Location.OldValue;
                nativity.Tags = (ObservableCollection<string>)info.Tags.OldValue;
                nativity.GeographicalOrigins = (ObservableCollection<string>)info.GeographicalOrigins.OldValue;
            }
        }
    }

    public class NativityEditedInfo
    {
        public NativityEditedInfo(NativityBase target)
        {
            this.target = target;
            id = new NativityPropertyChangeInfo(target);
            title = new NativityPropertyChangeInfo(target);
            origin = new NativityPropertyChangeInfo(target);
            acquired = new NativityPropertyChangeInfo(target);
            from = new NativityPropertyChangeInfo(target);
            cost = new NativityPropertyChangeInfo(target);
            location = new NativityPropertyChangeInfo(target);
            tags = new NativityPropertyChangeInfo(target);
            geographicalOrigins = new NativityPropertyChangeInfo(target);
        }

        NativityBase target;
        public NativityBase Target { get { return target; } }

        NativityPropertyChangeInfo id;
        public NativityPropertyChangeInfo Id { get { return id; } }

        NativityPropertyChangeInfo title;
        public NativityPropertyChangeInfo Title { get { return title; } }

        NativityPropertyChangeInfo origin;
        public NativityPropertyChangeInfo Origin { get { return origin; } }

        NativityPropertyChangeInfo acquired;
        public NativityPropertyChangeInfo Acquired { get { return acquired; } }

        NativityPropertyChangeInfo from;
        public NativityPropertyChangeInfo From { get { return from; } }

        NativityPropertyChangeInfo cost;
        public NativityPropertyChangeInfo Cost { get { return cost; } }

        NativityPropertyChangeInfo location;
        public NativityPropertyChangeInfo Location { get { return location; } }

        NativityPropertyChangeInfo tags;
        public NativityPropertyChangeInfo Tags { get { return tags; } }

        NativityPropertyChangeInfo geographicalOrigins;
        public NativityPropertyChangeInfo GeographicalOrigins { get { return geographicalOrigins; } }

        public void CopyCurrentValuesToOldValues()
        {
            id.OldValue = target.Id;
            title.OldValue = target.Title;
            origin.OldValue = target.Origin;
            acquired.OldValue = target.Acquired;
            from.OldValue = target.From;
            cost.OldValue = target.Cost;
            location.OldValue = target.Location;
            tags.OldValue = target.Tags;
            geographicalOrigins.OldValue = target.GeographicalOrigins;
        }

        public void CopyCurrentValuesToNewValues()
        {
            id.NewValue = target.Id;
            title.NewValue = target.Title;
            origin.NewValue = target.Origin;
            acquired.NewValue = target.Acquired;
            from.NewValue = target.From;
            cost.NewValue = target.Cost;
            location.NewValue = target.Location;
            tags.NewValue = target.Tags;
            geographicalOrigins.NewValue = target.GeographicalOrigins;
        }
    }

    public enum NativityProperty { Id, Title, Origin, Acquired, From, Cost, Location, Tags, GeographicalOrigins, Description }
}
