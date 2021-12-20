using System.Collections.Generic;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Interfaces.DistributionData
{
    public interface IDistributionDataService : IService
    {
        decimal ShareGlobal { get; }
        decimal ShareInType { get; }
        IEnumerable<IDistributionDataFab> DistributionDataFabs { get; }
    }
}