using System.Collections.Generic;
using System.Linq;
using Masterarbeit.Classes.DistributionData.Xml;
using Masterarbeit.Interfaces.DistributionData;

namespace Masterarbeit.Classes.DistributionData
{
    public class StatisticFromDeserializedStatistic : IStatistic
    {
        private readonly DistributionDataXml _distributionDataXml;
        private IEnumerable<IStatisticService> _services;

        public StatisticFromDeserializedStatistic(DistributionDataXml distributionDataXml)
        {
            _distributionDataXml = distributionDataXml;
        }

        public IEnumerable<IStatisticService> Services => ConvertedServices();

        private IEnumerable<IStatisticService> ConvertedServices()
        {
            if (_services != null)
                return _services;

            return _services = _distributionDataXml.MasterDataServices.Select(x => new StatisticServiceFromDeserializedStatistic(x));
        }
    }
}