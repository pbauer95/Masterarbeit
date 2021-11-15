using Masterarbeit.Interfaces.Fab;

namespace Masterarbeit.Interfaces.MasterData
{
    public interface IDistributionDataFab : IFab
    {
        decimal Share { get; }
    }
}