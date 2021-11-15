using System.Collections.Generic;
using System.Xml.Serialization;

namespace Masterarbeit.Classes.Attribute
{
    public class ValueSchemeXml
    {
        [XmlElement("Value")] public List<double> Values { get; set; }
    }
}