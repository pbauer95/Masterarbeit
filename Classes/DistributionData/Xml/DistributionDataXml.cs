using System.Collections.Generic;
using System.Xml.Serialization;

namespace Masterarbeit.Classes.DistributionData.Xml
{
    [XmlRoot("DistributionData")]
    public class DistributionDataXml
    {
        [XmlElement("DistributionDataService")] public List<DistributionDataServiceXml> MasterDataServices { get; set; }
    }
}