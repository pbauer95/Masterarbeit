using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Masterarbeit.Interfaces.BaseData;
using Masterarbeit.Interfaces.MasterData;
using Masterarbeit.Interfaces.Partition;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.Partition
{
    public class PartitionsFromServices : IEnumerable<IPartition>
    {
        private readonly IEnumerable<IService> _baseDataServices;
        private readonly IEnumerable<IDistributionDataService> _masterDataServices;
        private readonly int _count;
        private readonly bool _global;
        private IEnumerable<IPartition> _partitions;

        public PartitionsFromServices(IEnumerable<IService> baseDataServices, IEnumerable<IDistributionDataService> masterDataServices,
            int count,
            bool global)
        {
            _baseDataServices = baseDataServices;
            _masterDataServices = masterDataServices;
            _count = count;
            _global = global;
        }

        public IEnumerator<IPartition> GetEnumerator() => CalculatedPartitions().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<IPartition> CalculatedPartitions() => _partitions ??= DistributionService.DistributionService.AssignServicesToPartitions(
            _baseDataServices.ToList(), _masterDataServices.ToList(), _count, _global);
    }
}