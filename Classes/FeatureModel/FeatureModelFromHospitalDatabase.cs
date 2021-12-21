using System.Collections.Generic;
using Masterarbeit.Classes.Feature;
using Masterarbeit.Interfaces.Attribute;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.FeatureModel;
using Masterarbeit.Interfaces.HospitalData;

namespace Masterarbeit.Classes.FeatureModel
{
    public class FeatureModelFromHospitalDatabase : IFeatureModel
    {
        private readonly IHospitalDatabase _hospitalDatabase;
        private readonly IEnumerable<IAttribute> _attributes;
        private IEnumerable<IFeature> _features;

        public FeatureModelFromHospitalDatabase(IHospitalDatabase hospitalDatabase, IEnumerable<IAttribute> attributes)
        {
            _hospitalDatabase = hospitalDatabase;
            _attributes = attributes;
        }

        public IEnumerable<IFeature> Features => ConvertedFeatures();

        private IEnumerable<IFeature> ConvertedFeatures() => _features ??= new FeaturesFromHospitalDatabase(_hospitalDatabase, _attributes);
    }
}