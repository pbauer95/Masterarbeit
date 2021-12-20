using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.Partition;

namespace Masterarbeit.Classes.Partition
{
    public class PartitionsFromFeatures : IEnumerable<IPartition>
    {
        private readonly IEnumerable<IFeatureClassified> _features;
        private int _partitionCount;

        private IEnumerable<IPartition> _partitions;

        public PartitionsFromFeatures(IEnumerable<IFeatureClassified> features, int partitionCount)
        {
            _features = features;
            _partitionCount = partitionCount;
        }

        public IEnumerator<IPartition> GetEnumerator() => CalculatedPartitions().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<IPartition> CalculatedPartitions()
        {
            if (_partitions != null)
                return _partitions;

            var orderedFeatures = _features.OrderByDescending(x => x.Probability).ToList();

            if (_partitionCount > orderedFeatures.Count)
                _partitionCount = orderedFeatures.Count;

            var partitions = new IPartition[_partitionCount];

            var partitionSize = orderedFeatures.Count / _partitionCount;

            for (var i = 0; i < _partitionCount; i++)
            {
                var skip = i * partitionSize;
                var featurePartition = orderedFeatures.Skip(skip).Take(partitionSize).ToList();

                partitions[i] ??= new Partition(i, featurePartition);
            }

            return _partitions = partitions;
        }
    }
}