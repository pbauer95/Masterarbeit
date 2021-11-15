using System.Collections.Generic;

namespace Masterarbeit.Interfaces.MasterData
{
    public interface IMasterData
    {
        IEnumerable<IMasterDataService> Services { get; }
    }
}