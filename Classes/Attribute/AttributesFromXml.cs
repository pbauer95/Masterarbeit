using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Masterarbeit.Classes.Attribute.Xml;
using Masterarbeit.Interfaces.Attribute;

namespace Masterarbeit.Classes.Attribute
{
    public class AttributesFromXml : IEnumerable<IAttribute>
    {
        private readonly string _path;

        public AttributesFromXml(string path)
        {
            _path = path;
        }

        private IEnumerable<IAttribute> _attributes;
        public IEnumerator<IAttribute> GetEnumerator() => DeserializedAttributes().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<IAttribute> DeserializedAttributes()
        {
            if (_attributes != null)
                return _attributes;

            var reader = new System.Xml.Serialization.XmlSerializer(typeof(AttributesXml));
            var file = XDocument.Load(new System.IO.StreamReader(_path));

            _attributes = new AttributesFromDeserializedAttributes((AttributesXml)reader.Deserialize(file.CreateReader()));
            return _attributes;
        }
    }
}