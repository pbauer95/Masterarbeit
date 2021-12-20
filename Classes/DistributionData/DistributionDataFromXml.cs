﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Masterarbeit.Classes.DistributionData.Xml;
using Masterarbeit.Interfaces.DistributionData;

namespace Masterarbeit.Classes.DistributionData
{
    public class DistributionDataFromXml : IDistributionData
    {
        private string _path;
        private IDistributionData _distributionData;

        public DistributionDataFromXml(string path)
        {
            _path = path;
        }

        public IEnumerable<IDistributionDataService> Services => DeserializedDistributionData().Services;

        private IDistributionData DeserializedDistributionData()
        {
            if (_distributionData != null)
                return _distributionData;

            var reader = new System.Xml.Serialization.XmlSerializer(typeof(DistributionDataXml));
            var file = XDocument.Load(new System.IO.StreamReader(_path));

            _distributionData = new DistributionDataFromDeserializedDistributionData((DistributionDataXml)reader.Deserialize(file.CreateReader()));
            return _distributionData;
        }
    }
}