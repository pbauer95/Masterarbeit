using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Masterarbeit.Classes.Attribute.Xml;
using Masterarbeit.Interfaces.Attribute;

namespace Masterarbeit.Classes.Attribute
{
    public class AttributesFromDeserializedAttributes : IEnumerable<IAttribute>
    {
        private readonly AttributesXml _attributes;
        private IEnumerable<IAttribute> _convertedAttributes;

        public AttributesFromDeserializedAttributes(AttributesXml attributes)
        {
            _attributes = attributes;
        }

        public IEnumerator<IAttribute> GetEnumerator() => ConvertedAttributes().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<IAttribute> ConvertedAttributes()
        {
            if (_convertedAttributes != null)
                return _convertedAttributes;

            return _convertedAttributes = _attributes.Attributes.Select(x => new AttributeFromDeserializedAttribute(x));
        }
    }
}