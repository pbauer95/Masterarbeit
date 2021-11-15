using Masterarbeit.Interfaces.Attribute;

namespace Masterarbeit.Classes.Attribute
{
    public class Attribute : IAttribute
    {
        public Attribute(string name, IValueScheme valueScheme)
        {
            Name = name;
            ValueScheme = valueScheme;
        }

        public string Name { get; }
        public IValueScheme ValueScheme { get; }
    }
}