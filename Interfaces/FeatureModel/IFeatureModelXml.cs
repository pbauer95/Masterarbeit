using System.Xml.Linq;

namespace Masterarbeit.Interfaces.FeatureModel
{
    public interface IFeatureModelXml : IFeatureModel
    {
        XDocument ToXml();
    }
}