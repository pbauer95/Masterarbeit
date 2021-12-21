using System.Collections.Generic;
using System.Xml.Linq;
using Masterarbeit.Classes.DistributionData.Xml;
using Masterarbeit.Interfaces.DistributionData;

namespace Masterarbeit.Classes.DistributionData
{
    public class StatisticFromXml : IStatistic
    {
        private readonly string _path;
        private IStatistic _statistic;

        public StatisticFromXml(string path)
        {
            _path = path;
        }

        public IEnumerable<IStatisticService> Services => DeserializedDistributionData().Services;

        private IStatistic DeserializedDistributionData()
        {
            if (_statistic != null)
                return _statistic;

            var reader = new System.Xml.Serialization.XmlSerializer(typeof(DistributionDataXml));
            var file = XDocument.Load(new System.IO.StreamReader(_path));

            _statistic = new StatisticFromDeserializedStatistic((DistributionDataXml)reader.Deserialize(file.CreateReader()));
            return _statistic;
        }
    }
}