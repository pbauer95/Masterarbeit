using System;
using System.Collections.Generic;
using System.Linq;
using Masterarbeit.Classes.DistributionData.Xml;
using Masterarbeit.Interfaces.Fab;
using Masterarbeit.Interfaces.MasterData;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.DistributionData
{
    public class DistributionDataServiceFromDeserializedDistributionData : IDistributionDataService
    {
        private readonly DistributionDataServiceXml _distributionDataServiceXml;
        private IEnumerable<IDistributionDataFab> _fabs;

        public DistributionDataServiceFromDeserializedDistributionData(DistributionDataServiceXml distributionDataServiceXml)
        {
            _distributionDataServiceXml = distributionDataServiceXml;
        }

        public IService.ServiceType Type => _distributionDataServiceXml.Type.ToLower() switch
        {
            "ops" => IService.ServiceType.Ops,
            "drg" => IService.ServiceType.Drg,
            "mlg" => IService.ServiceType.Mlg,
            "mdc" => IService.ServiceType.Mdc,
            _ => throw new ArgumentOutOfRangeException()
        };

        public string Code => _distributionDataServiceXml.Code.Replace(" ", String.Empty).Replace("-", String.Empty).Replace(",", String.Empty);
        public IEnumerable<IFab> Fabs => MasterDataFabs;
        public decimal ShareGlobal => _distributionDataServiceXml.ShareGlobal;
        public decimal ShareInType => _distributionDataServiceXml.ShareInType;

        public IEnumerable<IDistributionDataFab> MasterDataFabs => _fabs ??= _distributionDataServiceXml.MasterDataFabs
            .Select(x => new DistributionDataFabFromDeserializedDistributionDataService(x));
    }
}