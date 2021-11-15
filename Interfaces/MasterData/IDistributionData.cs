using System.Collections.Generic;

namespace Masterarbeit.Interfaces.MasterData
{
    public interface IDistributionData
    {
        IEnumerable<IDistributionDataService> Services { get; }
    }
}