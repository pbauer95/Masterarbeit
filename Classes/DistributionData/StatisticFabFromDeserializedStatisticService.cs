using System;
using Masterarbeit.Classes.DistributionData.Xml;
using Masterarbeit.Interfaces.DistributionData;

namespace Masterarbeit.Classes.DistributionData
{
    public class StatisticFabFromDeserializedStatisticService : IStatisticFab
    {
        private readonly DistributionDataFabXml _fabXml;

        public StatisticFabFromDeserializedStatisticService(DistributionDataFabXml fabXml)
        {
            _fabXml = fabXml;
        }

        public string Name => _fabXml.Name.Replace(" ", String.Empty);
        public decimal CaseMix => new Random().Next(0, 500);
        public decimal Share => _fabXml.Share;
    }
}