using System.Collections.Generic;
using System.Xml.Serialization;

namespace Masterarbeit.Classes.MasterData.Xml
{
    [XmlRoot("MasterData")]
    public class MasterDataXml
    {
        [XmlElement("MasterDataService")] public List<MasterDataServiceXml> MasterDataServices { get; set; }
    }
}