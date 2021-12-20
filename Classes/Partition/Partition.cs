using System.Collections.Generic;
using System.Linq;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.Partition;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.Partition
{
    public class Partition : IPartition
    {
        private List<IFeature> _features;

        public Partition(int id, IEnumerable<IFeature> features)
        {
            Id = id;
            _features = features.ToList();
        }

        public Partition(int id)
        {
            Id = id;
            _features = new List<IFeature>();
        }

        public int Id { get; }
        public IList<IFeature> Features => _features;
    }
}