using System;
using System.Collections.Generic;
using System.Linq;
using Masterarbeit.Classes.Fab;
using Masterarbeit.Classes.HospitalData.Xml;
using Masterarbeit.Interfaces.Fab;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.Service
{
    public class ServiceFromDeserializedBaseData : IService
    {
        private readonly HospitalDataServiceXml _hospitalDataServiceXml;
        private IEnumerable<IFab> _fabs;

        public ServiceFromDeserializedBaseData(HospitalDataServiceXml hospitalDataServiceXml)
        {
            _hospitalDataServiceXml = hospitalDataServiceXml;
        }

        public IService.ServiceType Type => _hospitalDataServiceXml.Type.ToLower() switch
        {
            "ops" => IService.ServiceType.Ops,
            "drg" => IService.ServiceType.Drg,
            "mlg" => IService.ServiceType.Mlg,
            "mdc" => IService.ServiceType.Mdc,
            _ => throw new ArgumentOutOfRangeException()
        };

        public string Code => _hospitalDataServiceXml.Code.Replace(" ", String.Empty).Replace("-", String.Empty).Replace(",", String.Empty);
        public IEnumerable<IFab> Fabs => _fabs ??= _hospitalDataServiceXml.Fabs.Select(x => new FabFromDeserializedBaseDataService(x));
    }
}