using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Masterarbeit.Interfaces.DistributionData;
using Masterarbeit.Interfaces.Feature;

namespace Masterarbeit.Classes.Feature
{
    public class ClassifiedFeatures : IEnumerable<IFeatureClassified>
    {
        private readonly IEnumerable<IFeature> _features;
        private readonly IDistributionData _distributionData;
        private IEnumerable<IFeatureClassified> _classifiedFeatures;

        public ClassifiedFeatures(IEnumerable<IFeature> features, IDistributionData distributionData)
        {
            _features = features;
            _distributionData = distributionData;
        }

        public IEnumerator<IFeatureClassified> GetEnumerator() => FeaturesWithClassification().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<IFeatureClassified> FeaturesWithClassification()
        {
            if (_classifiedFeatures != null)
                return _classifiedFeatures;

            var distributionDictionary = _distributionData.Services.ToDictionary(x => new { x.Type, x.Code });

            var classifiedFeatures = (from feature in _features
                where feature.Service != null && !(!feature.Global && feature.Fab == null)
                let distribution = distributionDictionary[new { feature.Service.Type, feature.Service.Code }]
                let probability = (double)distribution.ShareInType * 10000000
                select new ClassifiedFeature(feature, probability)).Cast<IFeatureClassified>().ToList();

            return _classifiedFeatures = classifiedFeatures;
        }
    }
}