using System.Collections.Generic;
using System.Linq;
using Masterarbeit.Classes.MasterData.Xml;
using Masterarbeit.Interfaces.MasterData;

namespace Masterarbeit.Classes.MasterData
{
    public class MasterDataFromDeserializedMasterData : IMasterData
    {
        private readonly MasterDataXml _masterDataXml;
        private IEnumerable<IMasterDataService> _services;

        public MasterDataFromDeserializedMasterData(MasterDataXml masterDataXml)
        {
            _masterDataXml = masterDataXml;
        }

        public IEnumerable<IMasterDataService> Services => ConvertedServices();

        private IEnumerable<IMasterDataService> ConvertedServices()
        {
            if (_services != null)
                return _services;

            return _services = _masterDataXml.MasterDataServices.Select(x => new MasterDataServiceFromDeserializedMasterData(x));
        }
    }
}