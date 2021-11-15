using System.Collections.Generic;
using System.Xml.Serialization;

namespace Masterarbeit.Classes.MasterData.Xml
{
    public class MasterDataServiceXml
    {
        [XmlElement("Type")] public string Type { get; set; }
        [XmlElement("Code")] public string Code { get; set; }
        [XmlElement("ShareGlobal")] public decimal ShareGlobal { get; set; }
        [XmlElement("ShareInType")] public decimal ShareInType { get; set; }
        [XmlElement("MasterDataFab")] public List<MasterDataFabXml> MasterDataFabs { get; set; }
    }
}