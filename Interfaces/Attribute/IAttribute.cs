namespace Masterarbeit.Interfaces.Attribute
{
    public interface IAttribute
    {
        string Name { get; }
        IValueScheme ValueScheme { get; }
    }
}