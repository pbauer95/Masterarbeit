using System.Collections.Generic;
using System.Xml.Serialization;

namespace Masterarbeit.Classes.MasterData.Xml
{
    [XmlRoot("MasterData")]
    public class DistributionDataXml
    {
        [XmlElement("MasterDataService")] public List<DistributionDataServiceXml> MasterDataServices { get; set; }
    }
}