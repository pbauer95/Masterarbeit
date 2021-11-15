using System.Collections.Generic;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Interfaces.MasterData
{
    public interface IMasterDataService : IService
    {
        decimal ShareGlobal { get; }
        decimal ShareInType { get; }
        IEnumerable<IMasterDataFab> MasterDataFabs { get; }
    }
}