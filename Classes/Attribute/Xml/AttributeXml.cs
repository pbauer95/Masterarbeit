using System.Xml.Serialization;

namespace Masterarbeit.Classes.Attribute.Xml
{
    [XmlRoot("Attributes")]
    [XmlType("Attribute")]
    public class AttributeXml
    {
        [XmlElement("Name")] public string Name { get; set; }
        [XmlElement("ValueScheme")] public ValueSchemeXml ValueSchemeXml { get; set; }
    }
}