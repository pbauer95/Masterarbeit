using System.Collections.Generic;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Interfaces.Partition
{
    public interface IPartition
    {
        int Id { get; }
        IList<IService> Services { get; }
        void AddServices(IEnumerable<IService> services);
    }
}