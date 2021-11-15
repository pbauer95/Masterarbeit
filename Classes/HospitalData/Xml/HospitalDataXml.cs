using System.Collections.Generic;
using System.Xml.Serialization;

namespace Masterarbeit.Classes.HospitalData.Xml
{
    [XmlRoot("HospitalData")]
    public class HospitalDataXml
    {
        [XmlElement("HospitalDataService")] public List<HospitalDataServiceXml> HospitalDataServices { get; set; }
    }
}