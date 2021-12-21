using Masterarbeit.Interfaces.Fab;

namespace Masterarbeit.Interfaces.DistributionData
{
    public interface IStatisticFab : IFab
    {
        decimal Share { get; }
    }
}