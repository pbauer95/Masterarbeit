using System.Collections.Generic;
using Masterarbeit.Interfaces.Fab;

namespace Masterarbeit.Interfaces.Service
{
    public interface IService
    {
        enum ServiceType
        {
            Ops,
            Drg,
            Mlg,
            Mdc,
            Fab
        }

        ServiceType Type { get; }
        string Code { get; }
        IEnumerable<IFab> Fabs { get; }
    }
}