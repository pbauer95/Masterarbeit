using System.Collections.Generic;
using System.Linq;
using Masterarbeit.Interfaces.Attribute;
using Masterarbeit.Interfaces.Fab;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.Feature
{
    public class FeatureWithAttributes : IFeature
    {
        private readonly IFeature _feature;

        public FeatureWithAttributes(IFeature feature, IEnumerable<IAttribute> attributes)
        {
            Attributes = attributes;
            _feature = feature;
        }

        public IService Service => _feature.Service;
        public IEnumerable<IAttribute> Attributes { get; }
        public decimal CaseMix => Service.Fabs.Sum(x => x.CaseMix);
    }
}