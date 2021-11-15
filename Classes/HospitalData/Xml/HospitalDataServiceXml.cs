using System.Collections.Generic;
using System.Xml.Serialization;

namespace Masterarbeit.Classes.HospitalData.Xml
{
    public class HospitalDataServiceXml
    {
        [XmlElement("Type")] public string Type { get; set; }
        [XmlElement("Code")] public string Code { get; set; }
        [XmlElement("Fab")] public List<HospitalDataFabXml> Fabs { get; set; }
    }
}