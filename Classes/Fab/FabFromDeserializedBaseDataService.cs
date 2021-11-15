using System;
using Masterarbeit.Classes.HospitalData.Xml;
using Masterarbeit.Interfaces.Fab;

namespace Masterarbeit.Classes.Fab
{
    public class FabFromDeserializedBaseDataService : IFab
    {
        private readonly HospitalDataFabXml _hospitalDataFabXml;

        public FabFromDeserializedBaseDataService(HospitalDataFabXml hospitalDataFabXml)
        {
            _hospitalDataFabXml = hospitalDataFabXml;
        }

        public string Name => _hospitalDataFabXml.Name.Replace(" ", String.Empty);
        public decimal CaseMix => _hospitalDataFabXml.CaseMix;
    }
}