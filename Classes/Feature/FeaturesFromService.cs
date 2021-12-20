using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Masterarbeit.Interfaces.Attribute;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.Feature
{
    public class FeaturesFromService : IEnumerable<IFeature>
    {
        private readonly IService _service;
        private readonly IEnumerable<IAttribute> _attributes;
        private IEnumerable<IFeature> _features;

        public FeaturesFromService(IService service, IEnumerable<IAttribute> attributes)
        {
            _service = service;
            _attributes = attributes;
        }

        public IEnumerator<IFeature> GetEnumerator() => ConvertedFeatures().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<IFeature> ConvertedFeatures()
        {
            if (_features != null)
                return _features;

            if (!_service.Fabs.Any())
                throw new InvalidDataException(
                    "Medizinische Leistungen müssen in mindestens einer konkreten Fachabteilung erbracht werden.");

            var convertedFeatures = new List<IFeature>();

            //Globales Feature für medizinische Leistung
            convertedFeatures.Add(new Feature
            {
                Service = _service,
                Global = true,
                Attributes = _attributes,
                CaseMix = _service.Fabs.Sum(x => x.CaseMix),
            });

            //Globales Feature Einfrieren für medizinische Leistung
            convertedFeatures.Add(new Feature
            {
                Service = _service,
                Global = true,
                Freeze = true,
            });

            //Feature Abstraktionsebenen für medizinische Leistung
            convertedFeatures.Add(new Feature
            {
                Service = _service,
                AbstractionLevel = true,
                Global = false,
            });

            //Feature abstraktes Feature für konkrete Erbringung medizinische Leistung
            convertedFeatures.Add(new Feature
            {
                Service = _service,
                AbstractionLevel = false,
                Global = false,
            });

            foreach (var fab in _service.Fabs)
            {
                //Feature Abstraktionsebene Fachabteilung für medizinische Leistung
                convertedFeatures.Add(new Feature
                {
                    Service = _service,
                    AbstractionLevel = true,
                    Fab = fab,
                    Attributes = _attributes,
                    CaseMix = fab.CaseMix
                });

                //Feature Abstraktionsebene Fachabteilung Einfrieren für medizinische Leistung
                convertedFeatures.Add(new Feature
                {
                    Service = _service,
                    AbstractionLevel = true,
                    Freeze = true,
                    Fab = fab,
                });

                //Feature konkrete Leistungserbringung für medizinische Leistung
                convertedFeatures.Add(new Feature
                {
                    Service = _service,
                    AbstractionLevel = false,
                    Fab = fab,
                    Attributes = _attributes,
                    CaseMix = fab.CaseMix
                });

                //Feature konkrete Leistungserbringung Einfrieren für medizinische Leistung
                convertedFeatures.Add(new Feature
                {
                    Service = _service,
                    AbstractionLevel = false,
                    Freeze = true,
                    Fab = fab,
                });
            }

            return _features = convertedFeatures;
        }
    }
}