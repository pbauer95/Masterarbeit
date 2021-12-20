using System.Collections.Generic;
using Masterarbeit.Interfaces.BaseData;
using Masterarbeit.Interfaces.DistributionData;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.HospitalData
{
    public class HospitalDataFromDistributionData : IHospitalData
    {
        private readonly IDistributionData _distributionData;

        public HospitalDataFromDistributionData(IDistributionData distributionData)
        {
            _distributionData = distributionData;
        }

        public IEnumerable<IService> Services => _distributionData.Services;
    }
}