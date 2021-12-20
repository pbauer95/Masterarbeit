using System.Collections.Generic;
using Masterarbeit.Classes.Feature;
using Masterarbeit.Interfaces.Attribute;
using Masterarbeit.Interfaces.BaseData;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.FeatureModel;

namespace Masterarbeit.Classes.FeatureModel
{
    public class FeatureModelFromHospitalData : IFeatureModel
    {
        private readonly IHospitalData _hospitalData;
        private readonly IEnumerable<IAttribute> _attributes;
        private IEnumerable<IFeature> _features;

        public FeatureModelFromHospitalData(IHospitalData hospitalData, IEnumerable<IAttribute> attributes)
        {
            _hospitalData = hospitalData;
            _attributes = attributes;
        }

        public IEnumerable<IFeature> Features => ConvertedFeatures();

        private IEnumerable<IFeature> ConvertedFeatures() => _features ??= new FeaturesFromHospitalData(_hospitalData, _attributes);
    }
}