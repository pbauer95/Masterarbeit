﻿using System.Collections.Generic;
using Masterarbeit.Classes.HospitalData;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.MasterData;
using Masterarbeit.Interfaces.Partition;

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

        public IEnumerable<IPartition> GlobalPartitions => PartitionData().GlobalPartitions;
        public IEnumerable<IPartition> OpsPartitions => PartitionData().OpsPartitions;
        public IEnumerable<IPartition> DrgPartitions => PartitionData().DrgPartitions;
        public IEnumerable<IPartition> MlgPartitions => PartitionData().MlgPartitions;
        public IEnumerable<IPartition> MdcPartitions => PartitionData().MdcPartitions;

        public IEnumerable<IFeature> SelectFeaturesFromPartitions(IEnumerable<int> partitionIds, int maxSelected, bool global) =>
            PartitionData().SelectFeaturesFromPartitions(partitionIds, maxSelected, global);

        private IPartitionData PartitionData() =>
            _partitionData ??= new PartitionDataFromHospitalData(new HospitalDataFromMasterData(_distributionData), _distributionData, _partitionCount);
    }
}