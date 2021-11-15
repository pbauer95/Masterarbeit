using System;
using Masterarbeit.Classes.DistributionData.Xml;
using Masterarbeit.Interfaces.MasterData;

namespace Masterarbeit.Classes.DistributionData
{
    public class DistributionDataFabFromDeserializedDistributionDataService : IDistributionDataFab
    {
        private readonly DistributionDataFabXml _fabXml;

        public DistributionDataFabFromDeserializedDistributionDataService(DistributionDataFabXml fabXml)
        {
            _fabXml = fabXml;
        }

        public string Name => _fabXml.Name.Replace(" ", String.Empty);
        public decimal CaseMix => new Random().Next(0, 500);
        public decimal Share => _fabXml.Share;
    }
}