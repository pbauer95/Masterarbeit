using System.Collections.Generic;
using Masterarbeit.Interfaces.DistributionData;
using Masterarbeit.Interfaces.HospitalData;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.HospitalData
{
    public class HospitalDatabaseFromStatistic : IHospitalDatabase
    {
        private readonly IStatistic _statistic;

        public HospitalDatabaseFromStatistic(IStatistic statistic)
        {
            _statistic = statistic;
        }

        public IEnumerable<IService> Services => _statistic.Services;
    }
}