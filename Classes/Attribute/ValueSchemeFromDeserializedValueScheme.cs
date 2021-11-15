using System.Collections.Generic;
using Masterarbeit.Interfaces.Attribute;

namespace Masterarbeit.Classes.Attribute
{
    public class ValueSchemeFromDeserializedValueScheme : IValueScheme
    {
        private readonly ValueSchemeXml _valueSchemeXml;
        private IList<double> _values;

        public ValueSchemeFromDeserializedValueScheme(ValueSchemeXml valueSchemeXml)
        {
            _valueSchemeXml = valueSchemeXml;
        }

        public IList<double> Values => _values ??= _valueSchemeXml.Values;
    }
}