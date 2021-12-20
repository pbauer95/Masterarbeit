using System.Collections.Generic;
using Masterarbeit.Classes.HospitalData;
using Masterarbeit.Interfaces.Attribute;
using Masterarbeit.Interfaces.DistributionData;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.FeatureModel;

namespace Masterarbeit.Classes.FeatureModel
{
    public class FeatureModelFromDistributionData : IFeatureModel
    {
        private readonly IDistributionData _distributionData;
        private readonly IEnumerable<IAttribute> _attributes;
        private IFeatureModel _featureModel;

        public FeatureModelFromDistributionData(IDistributionData distributionData, IEnumerable<IAttribute> attributes)
        {
            _distributionData = distributionData;
            _attributes = attributes;
        }

        public IEnumerable<IFeature> Features => ConvertedFeatureModel().Features;

        private IFeatureModel ConvertedFeatureModel() =>
            _featureModel ??= new FeatureModelFromHospitalData(new HospitalDataFromDistributionData(_distributionData), _attributes);
    }
}