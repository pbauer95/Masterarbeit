using System.Collections.Generic;
using Masterarbeit.Interfaces.Attribute;
using Masterarbeit.Interfaces.Fab;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.Feature
{
    public class MandatoryFeature : IFeature
    {
        public MandatoryFeature(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public IService Service => null;
        public IFab Fab => null;
        public bool AbstractionLevel => false;
        public bool Global => false;
        public bool Freeze => false;
        public bool Mandatory => true;
        public IEnumerable<IAttribute> Attributes => null;
        public decimal CaseMix => 0;
    }
}