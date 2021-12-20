using System.Collections.Generic;
using Masterarbeit.Interfaces.Attribute;
using Masterarbeit.Interfaces.Fab;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.Feature
{
    public class Feature : IFeature
    {
        public IService Service { get; init; }
        public IFab Fab { get; init; }
        public bool AbstractionLevel { get; init; }
        public bool Global { get; init; }
        public bool Freeze { get; init; }
        public bool Mandatory => false;
        public IEnumerable<IAttribute> Attributes { get; init; }
        public decimal CaseMix { get; init; }
    }
}