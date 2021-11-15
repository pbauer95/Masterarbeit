using System.Collections.Generic;
using System.Linq;
using Masterarbeit.Classes.Attribute.Xml;
using Masterarbeit.Interfaces.Attribute;

namespace Masterarbeit.Classes.Attribute
{
    public class AttributeFromDeserializedAttribute : IAttribute
    {
        private readonly AttributeXml _attributeXml;
        private IValueScheme _valueScheme;

        public AttributeFromDeserializedAttribute(AttributeXml attributeXml)
        {
            _attributeXml = attributeXml;
        }

        public string Name => _attributeXml.Name;
        public IValueScheme ValueScheme => _valueScheme ??= new ValueSchemeFromDeserializedValueScheme(_attributeXml.ValueSchemeXml);
    }
}