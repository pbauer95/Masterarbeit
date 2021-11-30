using System.Collections.Generic;
using Masterarbeit.Interfaces.BaseData;
using Masterarbeit.Interfaces.MasterData;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.HospitalData
{
    public class HospitalDataFromMasterData : IHospitalData
    {
        private readonly IDistributionData _distributionData;

        public HospitalDataFromMasterData(IDistributionData distributionData)
        {
            _distributionData = distributionData;
        }

        public IEnumerable<IService> Services => _distributionData.Services;
    }
}