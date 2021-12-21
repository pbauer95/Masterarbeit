using System.Collections.Generic;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Interfaces.HospitalData
{
    public interface IHospitalDatabase
    {
        IEnumerable<IService> Services { get; }
    }
}