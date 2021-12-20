using Masterarbeit.Interfaces.Fab;

namespace Masterarbeit.Interfaces.DistributionData
{
    public interface IDistributionDataFab : IFab
    {
        decimal Share { get; }
    }
}