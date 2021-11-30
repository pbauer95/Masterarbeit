using System;
using System.Collections.Generic;
using System.Linq;
using Masterarbeit.Classes.Feature;
using Masterarbeit.Interfaces.BaseData;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.MasterData;
using Masterarbeit.Interfaces.Partition;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.Partition
{
    public class PartitionDataFromHospitalData : IPartitionData
    {
        private readonly IHospitalData _hospitalData;
        private readonly IDistributionData _distributionData;
        private readonly int _partitionCount;

        private IEnumerable<IPartition> _opsPartitions;
        private IEnumerable<IPartition> _drgPartitions;
        private IEnumerable<IPartition> _mlgPartitions;
        private IEnumerable<IPartition> _mdcPartitions;

        public PartitionDataFromHospitalData(IHospitalData hospitalData, IDistributionData distributionData, int partitionCount,
            Func<IList<IService>, IList<IDistributionDataService>, int, IEnumerable<IPartition>> partitionFunction)
        {
            _hospitalData = hospitalData;
            _distributionData = distributionData;
            _partitionCount = partitionCount;
            PartitionFunction = partitionFunction;
        }

        public IEnumerable<IPartition> OpsPartitions => CalculatedOpsPartitions();
        public IEnumerable<IPartition> DrgPartitions => CalculatedDrgPartitions();
        public IEnumerable<IPartition> MlgPartitions => CalculatedMlgPartitions();
        public IEnumerable<IPartition> MdcPartitions => CalculatedMdcPartitions();
        public Func<IList<IService>, IList<IDistributionDataService>, int, IEnumerable<IPartition>> PartitionFunction { get; }

        public IEnumerable<IFeature> SelectFeaturesFromPartitions(IEnumerable<int> partitionIds, int maxSelected)
        {
            return SelectedFeaturesFromTypePartitions(partitionIds.ToList(), maxSelected);
        }

        private IEnumerable<IPartition> CalculatedOpsPartitions() =>
            _opsPartitions ??= new PartitionsFromServices(_hospitalData.Services.Where(x => x.Type == IService.ServiceType.Ops),
                _distributionData.Services, _partitionCount, PartitionFunction);

        private IEnumerable<IPartition> CalculatedDrgPartitions() =>
            _drgPartitions ??= new PartitionsFromServices(_hospitalData.Services.Where(x => x.Type == IService.ServiceType.Drg),
                _distributionData.Services, _partitionCount, PartitionFunction);

        private IEnumerable<IPartition> CalculatedMlgPartitions() =>
            _mlgPartitions ??= new PartitionsFromServices(_hospitalData.Services.Where(x => x.Type == IService.ServiceType.Mlg),
                _distributionData.Services, _partitionCount, PartitionFunction);

        private IEnumerable<IPartition> CalculatedMdcPartitions() =>
            _mdcPartitions ??= new PartitionsFromServices(_hospitalData.Services.Where(x => x.Type == IService.ServiceType.Mdc),
                _distributionData.Services, _partitionCount, PartitionFunction);

        private IEnumerable<IFeature> SelectedFeaturesFromTypePartitions(IList<int> partitionIds, int maxSelected)
        {
            if (partitionIds.Any(x => x >= _partitionCount))
                throw new ArgumentOutOfRangeException("Partition nicht vorhanden");

            var selectedFeatures = new List<IFeature>();

            var index = 0;
            var selectionCount = 0;

            while (selectionCount < maxSelected && partitionIds.Any())
            {
                var partitionOps = OpsPartitions.ElementAt(partitionIds[index]);
                var partitionDrg = DrgPartitions.ElementAt(partitionIds[index]);
                var partitionMlg = MlgPartitions.ElementAt(partitionIds[index]);
                var partitionMdc = MdcPartitions.ElementAt(TransformIndexForMdc(partitionIds[index]));


                if (!partitionOps.Services.Any() && !partitionDrg.Services.Any() && !partitionMlg.Services.Any() &&
                    !partitionMdc.Services.Any())
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

        private int SelectFeatures(int maxSelected, int selectionCount, ICollection<IFeature> selectedFeatures,
            IPartition partitionOps, IPartition partitionDrg, IPartition partitionMlg, IPartition partitionMdc)
        {
            if (partitionOps.Services.Any() && selectionCount < maxSelected)
            {
                var service = RandomSelectedService(partitionOps);
                if (selectionCount + service.Fabs.Count() < maxSelected)
                {
                    selectedFeatures.Add(new FeatureFromService(service));
                    selectionCount += service.Fabs.Count();
                }

                partitionOps.Services.Remove(service);
            }

            if (partitionDrg.Services.Any() && selectionCount < maxSelected)
            {
                var service = RandomSelectedService(partitionDrg);
                if (selectionCount + service.Fabs.Count() < maxSelected)
                {
                    selectedFeatures.Add(new FeatureFromService(service));
                    selectionCount += service.Fabs.Count();
                }

                partitionDrg.Services.Remove(service);
            }

            if (partitionMlg.Services.Any() && selectionCount < maxSelected)
            {
                var service = RandomSelectedService(partitionMlg);
                if (selectionCount + service.Fabs.Count() < maxSelected)
                {
                    selectedFeatures.Add(new FeatureFromService(service));
                    selectionCount += service.Fabs.Count();
                }

                partitionMlg.Services.Remove(service);
            }

            if (partitionMdc.Services.Any() && selectionCount < maxSelected)
            {
                var service = RandomSelectedService(partitionMdc);
                if (selectionCount + service.Fabs.Count() < maxSelected)
                {
                    selectedFeatures.Add(new FeatureFromService(service));
                    selectionCount += service.Fabs.Count();
                }

                partitionMdc.Services.Remove(service);
            }

            return selectionCount;
        }

        private IService RandomSelectedService(IPartition partition)
        {
            var randomFeatureId = new Random().Next(0, partition.Services.Count - 1);
            return partition.Services[randomFeatureId];
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