using System.Collections.Generic;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Interfaces.MasterData
{
    public interface IDistributionDataService : IService
    {
        decimal ShareGlobal { get; }
        decimal ShareInType { get; }
        IEnumerable<IDistributionDataFab> MasterDataFabs { get; }
    }
}