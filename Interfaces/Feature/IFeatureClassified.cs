namespace Masterarbeit.Interfaces.Feature
{
    public interface IFeatureClassified : IFeature
    {
        double Probability { get; }
    }
}