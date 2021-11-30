using System;
using System.Collections.Generic;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.MasterData;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Interfaces.Partition
{
    public interface IPartitionData
    {
        IEnumerable<IPartition> OpsPartitions { get; }
        IEnumerable<IPartition> DrgPartitions { get; }
        IEnumerable<IPartition> MlgPartitions { get; }
        IEnumerable<IPartition> MdcPartitions { get; }
        
        Func<IList<IService>, IList<IDistributionDataService>, int, IEnumerable<IPartition>> PartitionFunction { get; }

        IEnumerable<IFeature> SelectFeaturesFromPartitions(IEnumerable<int> partitionIds, int maxSelected);
    }
}