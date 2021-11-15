using System.Collections.Generic;
using System.Xml.Serialization;

namespace Masterarbeit.Classes.Attribute.Xml
{
    [XmlRoot("Attributes")]
    public class AttributesXml
    {
        [XmlElement("Attribute")] public List<AttributeXml> Attributes { get; set; }
    }
}