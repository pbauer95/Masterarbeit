using System.Collections.Generic;

namespace Masterarbeit.Interfaces.DistributionData
{
    public interface IDistributionData
    {
        IEnumerable<IDistributionDataService> Services { get; }
    }
}