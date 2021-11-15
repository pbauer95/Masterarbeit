using System;
using Masterarbeit.Classes.MasterData.Xml;
using Masterarbeit.Interfaces.MasterData;

namespace Masterarbeit.Classes.MasterData
{
    public class MasterDataFabFromDeserializedMasterDataService : IMasterDataFab
    {
        private readonly MasterDataFabXml _fabXml;

        public MasterDataFabFromDeserializedMasterDataService(MasterDataFabXml fabXml)
        {
            _fabXml = fabXml;
        }

        public string Name => _fabXml.Name.Replace(" ", String.Empty);
        public decimal CaseMix => new Random().Next(0, 500);
        public decimal Share => _fabXml.Share;
    }
}