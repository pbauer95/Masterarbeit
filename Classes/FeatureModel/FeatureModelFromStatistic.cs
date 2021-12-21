using System.Collections.Generic;
using Masterarbeit.Classes.HospitalData;
using Masterarbeit.Interfaces.Attribute;
using Masterarbeit.Interfaces.DistributionData;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.FeatureModel;

namespace Masterarbeit.Classes.FeatureModel
{
    public class FeatureModelFromStatistic : IFeatureModel
    {
        private readonly IStatistic _statistic;
        private readonly IEnumerable<IAttribute> _attributes;
        private IFeatureModel _featureModel;

        public FeatureModelFromStatistic(IStatistic statistic, IEnumerable<IAttribute> attributes)
        {
            _statistic = statistic;
            _attributes = attributes;
        }

        public IEnumerable<IFeature> Features => ConvertedFeatureModel().Features;

        private IFeatureModel ConvertedFeatureModel() =>
            _featureModel ??= new FeatureModelFromHospitalDatabase(new HospitalDatabaseFromStatistic(_statistic), _attributes);
    }
}