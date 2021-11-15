using System.Collections.Generic;
using System.Xml.Serialization;

namespace Masterarbeit.Classes.DistributionData.Xml
{
    [XmlRoot("MasterData")]
    public class DistributionDataXml
    {
        [XmlElement("MasterDataService")] public List<DistributionDataServiceXml> MasterDataServices { get; set; }
    }
}