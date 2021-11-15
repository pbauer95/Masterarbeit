using Masterarbeit.Interfaces.Fab;

namespace Masterarbeit.Interfaces.MasterData
{
    public interface IMasterDataFab : IFab
    {
        decimal Share { get; }
    }
}