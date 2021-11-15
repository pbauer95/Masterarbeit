using System;
using System.Collections.Generic;
using System.Linq;
using Masterarbeit.Classes.MasterData.Xml;
using Masterarbeit.Interfaces.Fab;
using Masterarbeit.Interfaces.MasterData;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.MasterData
{
    public class MasterDataServiceFromDeserializedMasterData : IMasterDataService
    {
        private readonly MasterDataServiceXml _masterDataServiceXml;
        private IEnumerable<IMasterDataFab> _fabs;

        public MasterDataServiceFromDeserializedMasterData(MasterDataServiceXml masterDataServiceXml)
        {
            _masterDataServiceXml = masterDataServiceXml;
        }

        public IService.ServiceType Type => _masterDataServiceXml.Type.ToLower() switch
        {
            "ops" => IService.ServiceType.Ops,
            "drg" => IService.ServiceType.Drg,
            "mlg" => IService.ServiceType.Mlg,
            "mdc" => IService.ServiceType.Mdc,
            _ => throw new ArgumentOutOfRangeException()
        };

        public string Code => _masterDataServiceXml.Code.Replace(" ", String.Empty).Replace("-", String.Empty).Replace(",", String.Empty);
        public IEnumerable<IFab> Fabs => MasterDataFabs;
        public decimal ShareGlobal => _masterDataServiceXml.ShareGlobal;
        public decimal ShareInType => _masterDataServiceXml.ShareInType;

        public IEnumerable<IMasterDataFab> MasterDataFabs => _fabs ??= _masterDataServiceXml.MasterDataFabs
            .Select(x => new MasterDataFabFromDeserializedMasterDataService(x));
    }
}