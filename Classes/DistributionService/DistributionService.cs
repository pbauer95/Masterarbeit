using System.Collections.Generic;
using System.Linq;
using Masterarbeit.Interfaces.MasterData;
using Masterarbeit.Interfaces.Partition;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.DistributionService
{
    public static class DistributionService
    {
        //TODO: Zuteilungsalgorithmus
        public static IEnumerable<IPartition> AssignServicesToPartitions(IList<IService> services, IList<IMasterDataService> masterData,
            int count, bool global)
        {
            var existingServices = services.Count != masterData.Count
                ? masterData.Where(x => services.Any(y => y.Type == x.Type && y.Code == x.Code)).ToList()
                : masterData.ToList();

            existingServices = global
                ? existingServices.OrderBy(x => x.ShareGlobal).ToList()
                : existingServices.OrderBy(x => x.ShareInType).ToList();

            if (count > existingServices.Count)
                count = existingServices.Count;

            var partitions = new IPartition[count];

            var partitionSize = existingServices.Count / count;

            for (var i = 0; i < count; i++)
            {
                var skip = i * partitionSize;
                var partitionMasterServices = existingServices.Skip(skip).Take(partitionSize).ToList();

                partitions[i] ??= new Partition.Partition(i,
                    services.Where(x => partitionMasterServices.Any(y => y.Type == x.Type && y.Code == x.Code)));
            }

            return partitions;
        }
    }
}