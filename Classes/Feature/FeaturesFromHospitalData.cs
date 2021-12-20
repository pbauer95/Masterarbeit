using System.Collections;
using System.Collections.Generic;
using Masterarbeit.Interfaces.Attribute;
using Masterarbeit.Interfaces.BaseData;
using Masterarbeit.Interfaces.Feature;

namespace Masterarbeit.Classes.Feature
{
    public class FeaturesFromHospitalData : IEnumerable<IFeature>
    {
        private readonly IHospitalData _hospitalData;
        private readonly IEnumerable<IAttribute> _attributes;
        private IEnumerable<IFeature> _features;

        public FeaturesFromHospitalData(IHospitalData hospitalData, IEnumerable<IAttribute> attributes)
        {
            _hospitalData = hospitalData;
            _attributes = attributes;
        }

        public IEnumerator<IFeature> GetEnumerator() => ConvertedFeatures().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<IFeature> ConvertedFeatures()
        {
            if (_features != null)
                return _features;

            var convertedFeatures = new List<IFeature>
            {
                new MandatoryFeature("LeistungenOps"),
                new MandatoryFeature("LeistungenDrg"),
                new MandatoryFeature("LeistungenMlg"),
                new MandatoryFeature("LeistungenMdc"),
                new MandatoryFeature("LeistungenFab"),
                new MandatoryFeature("Leistungen"),
                new MandatoryFeature("Jahresplanung"),
                new MandatoryFeature("PLATO"),
            };

            foreach (var service in _hospitalData.Services)
            {
                convertedFeatures.AddRange(new FeaturesFromService(service, _attributes));
            }

            return _features = convertedFeatures;
        }
    }
}