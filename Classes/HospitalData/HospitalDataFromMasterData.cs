using System.Collections.Generic;
using Masterarbeit.Interfaces.BaseData;
using Masterarbeit.Interfaces.MasterData;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.HospitalData
{
    public class HospitalDataFromMasterData : IHospitalData
    {
        private readonly IMasterData _masterData;

        public HospitalDataFromMasterData(IMasterData masterData)
        {
            _masterData = masterData;
        }

        public IEnumerable<IService> Services => _masterData.Services;
    }
}