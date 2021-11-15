using System.Collections.Generic;
using Masterarbeit.Interfaces.Attribute;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.Feature
{
    public class FeatureFromService : IFeature
    {
        public FeatureFromService(IService service)
        {
            Service = service;
        }

        public IService Service { get; }
        public IEnumerable<IAttribute> Attributes => new List<IAttribute>();
        public decimal CaseMix => 0;
    }
}