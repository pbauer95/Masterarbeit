using System.Collections.Generic;

namespace Masterarbeit.Interfaces.DistributionData
{
    public interface IStatistic
    {
        IEnumerable<IStatisticService> Services { get; }
    }
}