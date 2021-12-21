using System.Collections.Generic;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Interfaces.DistributionData
{
    public interface IStatisticService : IService
    {
        decimal ShareGlobal { get; }
        decimal ShareInType { get; }
        IEnumerable<IStatisticFab> DistributionDataFabs { get; }
    }
}