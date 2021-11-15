using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Masterarbeit.Interfaces.Attribute;
using Masterarbeit.Interfaces.Feature;

namespace Masterarbeit.Classes.Feature
{
    public class FeaturesWithFabsAndAttributes : IEnumerable<IFeature>
    {
        private readonly IEnumerable<IFeature> _features;
        private readonly IEnumerable<IAttribute> _attributes;
        private IEnumerable<IFeature> _featuresWithAttributes;

        public FeaturesWithFabsAndAttributes(IEnumerable<IFeature> features, IEnumerable<IAttribute> attributes)
        {
            _features = features;
            _attributes = attributes;
        }

        public IEnumerator<IFeature> GetEnumerator() => ConvertedFeatures().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<IFeature> ConvertedFeatures()
        {
            if (_featuresWithAttributes != null)
                return _featuresWithAttributes;

            return _featuresWithAttributes =
                (from feature in _features
                    select new FeatureWithAttributes(feature, _attributes)).ToList();
        }
    }
}