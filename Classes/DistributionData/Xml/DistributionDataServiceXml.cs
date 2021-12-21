using System.Collections.Generic;
using System.Xml.Serialization;

namespace Masterarbeit.Classes.DistributionData.Xml
{
    public class DistributionDataServiceXml
    {
        [XmlElement("Type")] public string Type { get; set; }
        [XmlElement("Code")] public string Code { get; set; }
        [XmlElement("ShareGlobal")] public decimal ShareGlobal { get; set; }
        [XmlElement("ShareInType")] public decimal ShareInType { get; set; }
        [XmlElement("DistributionDataFab")] public List<DistributionDataFabXml> MasterDataFabs { get; set; }
    }
}