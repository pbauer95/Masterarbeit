using System.Collections.Generic;
using Masterarbeit.Interfaces.Feature;

namespace Masterarbeit.Interfaces.Partition
{
    public interface IPartition
    {
        int Id { get; }
        IList<IFeature> Features { get; }
    }
}