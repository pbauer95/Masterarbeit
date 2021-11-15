using System.Collections.Generic;
using System.Linq;
using Masterarbeit.Interfaces.Partition;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.Partition
{
    public class Partition : IPartition
    {
        private List<IService> _services;

        public Partition(int id, IEnumerable<IService> services)
        {
            Id = id;
            _services = services.ToList();
        }

        public Partition(int id)
        {
            Id = id;
            _services = new List<IService>();
        }

        public int Id { get; }
        public IList<IService> Services => _services;

        public void AddServices(IEnumerable<IService> service)
        {
            _services.AddRange(service);
        }
    }
}