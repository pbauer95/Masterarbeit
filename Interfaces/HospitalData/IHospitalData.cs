using System.Collections.Generic;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Interfaces.BaseData
{
    public interface IHospitalData
    {
        IEnumerable<IService> Services { get; }
    }
}