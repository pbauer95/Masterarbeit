using System.Collections.Generic;
using Masterarbeit.Interfaces.Fab;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.Service
{
    public class FabService : IService
    {
        public IService.ServiceType Type => IService.ServiceType.Fab;
        public string Code { get; init; }
        public IEnumerable<IFab> Fabs { get; init; }
    }
}