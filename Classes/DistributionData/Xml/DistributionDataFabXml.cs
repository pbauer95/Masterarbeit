using System.Xml.Serialization;

namespace Masterarbeit.Classes.DistributionData.Xml
{
    public class DistributionDataFabXml
    {
        [XmlElement("Name")] public string Name { get; set; }
        [XmlElement("Share")] public decimal Share { get; set; }
    }
}