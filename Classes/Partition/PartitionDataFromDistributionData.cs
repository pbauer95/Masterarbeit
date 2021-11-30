using System;
using System.Collections.Generic;
using Masterarbeit.Classes.HospitalData;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.MasterData;
using Masterarbeit.Interfaces.Partition;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.Partition
{
    public class PartitionDataFromMasterData : IPartitionData
    {
        private readonly IDistributionData _distributionData;
        private readonly int _partitionCount;
        private IPartitionData _partitionData;

        public PartitionDataFromMasterData(IDistributionData distributionData, int partitionCount)
        {
            _distributionData = distributionData;
            _partitionCount = partitionCount;
        }

        public IEnumerable<IPartition> OpsPartitions => PartitionData().OpsPartitions;
        public IEnumerable<IPartition> DrgPartitions => PartitionData().DrgPartitions;
        public IEnumerable<IPartition> MlgPartitions => PartitionData().MlgPartitions;
        public IEnumerable<IPartition> MdcPartitions => PartitionData().MdcPartitions;
        public Func<IList<IService>, IList<IDistributionDataService>, int, IEnumerable<IPartition>> PartitionFunction { get; set; }

        public IEnumerable<IFeature> SelectFeaturesFromPartitions(IEnumerable<int> partitionIds, int maxSelected) =>
            PartitionData().SelectFeaturesFromPartitions(partitionIds, maxSelected);

        private IPartitionData PartitionData() =>
            _partitionData ??=
                new PartitionDataFromHospitalData(new HospitalDataFromDistributionData(_distributionData), _distributionData, _partitionCount);
    }
}