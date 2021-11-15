using System.Xml.Serialization;

namespace Masterarbeit.Classes.HospitalData.Xml
{
    public class HospitalDataFabXml
    {
        [XmlElement("Name")] public string Name { get; set; }
        [XmlElement("CaseMix")] public decimal CaseMix { get; set; }
    }
}