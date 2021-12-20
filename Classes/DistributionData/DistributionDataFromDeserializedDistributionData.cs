using System.Collections.Generic;
using System.Linq;
using Masterarbeit.Classes.DistributionData.Xml;
using Masterarbeit.Interfaces.DistributionData;

namespace Masterarbeit.Classes.DistributionData
{
    public class DistributionDataFromDeserializedDistributionData : IDistributionData
    {
        private readonly DistributionDataXml _distributionDataXml;
        private IEnumerable<IDistributionDataService> _services;

        public DistributionDataFromDeserializedDistributionData(DistributionDataXml distributionDataXml)
        {
            _distributionDataXml = distributionDataXml;
        }

        public IEnumerable<IDistributionDataService> Services => ConvertedServices();

        private IEnumerable<IDistributionDataService> ConvertedServices()
        {
            if (_services != null)
                return _services;

            return _services = _distributionDataXml.MasterDataServices.Select(x => new DistributionDataServiceFromDeserializedDistributionData(x));
        }
    }
}