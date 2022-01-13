using System;
using System.Collections.Generic;
using Masterarbeit.Interfaces.DistributionData;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Interfaces.Partition
{
    public interface IPartitionData
    {
        IEnumerable<IPartition> OpsPartitions { get; }
        IEnumerable<IPartition> DrgPartitions { get; }
        IEnumerable<IPartition> MlgPartitions { get; }
        IEnumerable<IPartition> MdcPartitions { get; }

        IEnumerable<IFeature> SelectFeaturesFromPartitions(IEnumerable<int> partitionIds, int maxSelected);
    }
}