using System.Collections.Generic;
using Masterarbeit.Interfaces.Attribute;
using Masterarbeit.Interfaces.Fab;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.Feature
{
    public class ClassifiedFeature : IFeatureClassified
    {
        private readonly IFeature _feature;

        public ClassifiedFeature(IFeature feature, double probability)
        {
            Probability = probability;
            _feature = feature;
        }

        public IService Service => _feature.Service;
        public IFab Fab => _feature.Fab;
        public bool AbstractionLevel => _feature.AbstractionLevel;
        public bool Global => _feature.Global;
        public bool Freeze => _feature.Freeze;
        public bool Mandatory => _feature.Mandatory;
        public IEnumerable<IAttribute> Attributes => _feature.Attributes;
        public decimal CaseMix => _feature.CaseMix;
        public double Probability { get; }
    }
}