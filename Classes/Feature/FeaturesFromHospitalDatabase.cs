using System.Collections;
using System.Collections.Generic;
using Masterarbeit.Interfaces.Attribute;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.HospitalData;

namespace Masterarbeit.Classes.Feature
{
    public class FeaturesFromHospitalDatabase : IEnumerable<IFeature>
    {
        private readonly IHospitalDatabase _hospitalDatabase;
        private readonly IEnumerable<IAttribute> _attributes;
        private IEnumerable<IFeature> _features;

        public FeaturesFromHospitalDatabase(IHospitalDatabase hospitalDatabase, IEnumerable<IAttribute> attributes)
        {
            _hospitalDatabase = hospitalDatabase;
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

            foreach (var service in _hospitalDatabase.Services)
            {
                convertedFeatures.AddRange(new FeaturesFromService(service, _attributes));
            }

            return _features = convertedFeatures;
        }
    }
}