using System;
using System.Collections.Generic;
using System.Linq;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.Partition;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.Partition
{
    public class PartitionDataFromFeatures : IPartitionData
    {
        private readonly IEnumerable<IFeatureClassified> _classifiedFeatures;
        private readonly int _partitionCount;

        private IEnumerable<IPartition> _opsPartitions;
        private IEnumerable<IPartition> _drgPartitions;
        private IEnumerable<IPartition> _mlgPartitions;
        private IEnumerable<IPartition> _mdcPartitions;

        public PartitionDataFromFeatures(IEnumerable<IFeatureClassified> classifiedFeatures, int partitionCount)
        {
            _classifiedFeatures = classifiedFeatures;
            _partitionCount = partitionCount;
        }

        public IEnumerable<IPartition> OpsPartitions => CalculatedOpsPartitions();
        public IEnumerable<IPartition> DrgPartitions => CalculatedDrgPartitions();
        public IEnumerable<IPartition> MlgPartitions => CalculatedMlgPartitions();
        public IEnumerable<IPartition> MdcPartitions => CalculatedMdcPartitions();

        public IEnumerable<IFeature> SelectFeaturesFromPartitions(IList<int> partitionIds, int maxSelected)
        {
            if (partitionIds.Any(x => x >= _partitionCount))
                throw new ArgumentOutOfRangeException("Partition nicht vorhanden!");

            var selectedFeatures = new List<IFeature>();

            var index = 0;
            var selectionCount = 0;

            while (selectionCount < maxSelected && partitionIds.Any())
            {
                var partitionOps = OpsPartitions.ElementAt(partitionIds[index]);
                var partitionDrg = DrgPartitions.ElementAt(partitionIds[index]);
                var partitionMlg = MlgPartitions.ElementAt(partitionIds[index]);
                var partitionMdc = MdcPartitions.ElementAt(TransformIndexForMdc(partitionIds[index]));


                if (!partitionOps.Features.Any() && !partitionDrg.Features.Any() && !partitionMlg.Features.Any() &&
                    !partitionMdc.Features.Any())
                {
                    partitionIds.RemoveAt(index);
                    continue;
                }

                selectionCount = SelectFeatures(maxSelected, selectionCount, selectedFeatures,
                    partitionOps, partitionDrg, partitionMlg, partitionMdc);

                index++;

                if (index >= partitionIds.Count)
                    index = 0;
            }

            return selectedFeatures;
        }

        private IEnumerable<IPartition> CalculatedOpsPartitions() =>
            _opsPartitions ??= new PartitionsFromFeatures(
                _classifiedFeatures.Where(x => x.Service?.Type == IService.ServiceType.Ops), _partitionCount);

        private IEnumerable<IPartition> CalculatedDrgPartitions() =>
            _drgPartitions ??= new PartitionsFromFeatures(
                _classifiedFeatures.Where(x => x.Service?.Type == IService.ServiceType.Drg), _partitionCount);

        private IEnumerable<IPartition> CalculatedMlgPartitions() =>
            _mlgPartitions ??= new PartitionsFromFeatures(
                _classifiedFeatures.Where(x => x.Service?.Type == IService.ServiceType.Mlg), _partitionCount);

        private IEnumerable<IPartition> CalculatedMdcPartitions() =>
            _mdcPartitions ??= new PartitionsFromFeatures(
                _classifiedFeatures.Where(x => x.Service?.Type == IService.ServiceType.Mdc), _partitionCount);

        private int SelectFeatures(int maxSelected, int selectionCount, ICollection<IFeature> selectedFeatures,
            IPartition partitionOps, IPartition partitionDrg, IPartition partitionMlg, IPartition partitionMdc)
        {
            if (selectionCount >= maxSelected)
                return selectionCount;

            if (partitionOps.Features.Any() && selectionCount < maxSelected)
            {
                var feature = RandomSelectedFeature(partitionOps);
                selectedFeatures.Add(feature);
                selectionCount++;
                partitionOps.Features.Remove(feature);
            }

            if (partitionDrg.Features.Any() && selectionCount < maxSelected)
            {
                var feature = RandomSelectedFeature(partitionDrg);
                selectedFeatures.Add(feature);
                selectionCount++;
                partitionDrg.Features.Remove(feature);
            }

            if (partitionMlg.Features.Any() && selectionCount < maxSelected)
            {
                var feature = RandomSelectedFeature(partitionMlg);
                selectedFeatures.Add(feature);
                selectionCount++;
                partitionMlg.Features.Remove(feature);
            }

            if (!partitionMdc.Features.Any() || selectionCount >= maxSelected)
                return selectionCount;
            {
                var feature = RandomSelectedFeature(partitionMdc);
                selectedFeatures.Add(feature);
                selectionCount++;
                partitionMdc.Features.Remove(feature);
            }

            return selectionCount;
        }

        private IFeature RandomSelectedFeature(IPartition partition)
        {
            var randomFeatureId = new Random().Next(0, partition.Features.Count - 1);
            return partition.Features[randomFeatureId];
        }

        private int TransformIndexForMdc(int index)
        {
            if (MdcPartitions.Count() == OpsPartitions.Count())
                return index;

            var ratio = (double)MdcPartitions.Count() / OpsPartitions.Count();

            var transformedIndex = (int)Math.Round(ratio * index, 0);

            return transformedIndex >= MdcPartitions.Count() ? MdcPartitions.Count() - 1 : transformedIndex;
        }
    }
}