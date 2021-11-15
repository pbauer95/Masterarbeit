using System.Collections.Generic;
using Masterarbeit.Interfaces.Attribute;
using Masterarbeit.Interfaces.Fab;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Interfaces.Feature
{
    public interface IFeature
    {
        IService Service { get; }
        IEnumerable<IAttribute> Attributes { get; }
        decimal CaseMix { get; }
    }
}