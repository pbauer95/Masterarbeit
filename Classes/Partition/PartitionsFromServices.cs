using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Masterarbeit.Interfaces.DistributionData;
using Masterarbeit.Interfaces.Partition;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.Partition
{
    public class PartitionsFromServices : IEnumerable<IPartition>
    {
        private readonly IEnumerable<IService> _baseDataServices;
        private readonly IEnumerable<IDistributionDataService> _masterDataServices;
        private readonly int _count;
        private readonly Func<IList<IService>, IList<IDistributionDataService>, int, IEnumerable<IPartition>> _partitionFunction;
        private IEnumerable<IPartition> _partitions;

        public PartitionsFromServices(IEnumerable<IService> baseDataServices, IEnumerable<IDistributionDataService> masterDataServices,
            int count, Func<IList<IService>, IList<IDistributionDataService>, int, IEnumerable<IPartition>> partitionFunction)
        {
            _baseDataServices = baseDataServices;
            _masterDataServices = masterDataServices;
            _count = count;
            _partitionFunction = partitionFunction;
        }

        public IEnumerator<IPartition> GetEnumerator() => CalculatedPartitions().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<IPartition> CalculatedPartitions() => _partitions ??= _partitionFunction(
            _baseDataServices.ToList(), _masterDataServices.ToList(), _count);
    }
}