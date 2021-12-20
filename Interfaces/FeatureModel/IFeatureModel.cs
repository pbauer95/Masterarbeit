using System.Collections.Generic;
using System.Xml.Linq;
using Masterarbeit.Interfaces.Feature;

namespace Masterarbeit.Interfaces.FeatureModel
{
    public interface IFeatureModel
    {
        IEnumerable<IFeature> Features { get; }
    }
}