using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Masterarbeit.Interfaces.Attribute;
using Masterarbeit.Interfaces.Fab;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.FeatureModel;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.FeatureModel
{
    public class FeatureModelFromFeatures : IFeatureModel
    {
        private XDocument _xml;

        public FeatureModelFromFeatures(IEnumerable<IFeature> features)
        {
            Features = features;
        }

        public IEnumerable<IFeature> Features { get; }

        public XDocument ToXml()
        {
            if (_xml != null)
                return _xml;

            var constraints = FeaturesToConstraints(Features);

            var opsLeistungen = FeaturesToXml(Features.Where(x => x.Service.Type == IService.ServiceType.Ops).ToList(), constraints);
            var drgLeistungen = FeaturesToXml(Features.Where(x => x.Service.Type == IService.ServiceType.Drg).ToList(), constraints);
            var mlgLeistungen = FeaturesToXml(Features.Where(x => x.Service.Type == IService.ServiceType.Mlg).ToList(), constraints);
            var mdcLeistungen = FeaturesToXml(Features.Where(x => x.Service.Type == IService.ServiceType.Mdc).ToList(), constraints);
            var fabLeistungen = FabFeaturesToXml(Features.ToList(), constraints);

            var hospitalModifications = RandomXmlHospitalModifications();

            _xml = new XDocument(
                new XDeclaration("1.0", "UTF-8", "no"),
                new XElement("featureModel",
                    new XElement("struct",
                        new XElement("and", new XAttribute("abstract", true), new XAttribute("mandatory", true), new XAttribute("name", "PLATO"),
                            new XElement("and", new XAttribute("abstract", true), new XAttribute("mandatory", true),
                                new XAttribute("name", "Jahresplanung"),
                                new XElement("and", new XAttribute("abstract", true), new XAttribute("mandatory", true),
                                    new XAttribute("name", "Leistungen"),
                                    new XElement("or", new XAttribute("abstract", true), new XAttribute("mandatory", true),
                                        new XAttribute("name", "LeistungenOps"),
                                        opsLeistungen),
                                    new XElement("or", new XAttribute("abstract", true), new XAttribute("mandatory", true),
                                        new XAttribute("name", "LeistungenDrg"),
                                        drgLeistungen),
                                    new XElement("or", new XAttribute("abstract", true), new XAttribute("mandatory", true),
                                        new XAttribute("name", "LeistungenMlg"),
                                        mlgLeistungen),
                                    new XElement("or", new XAttribute("abstract", true), new XAttribute("mandatory", true),
                                        new XAttribute("name", "LeistungenMdc"),
                                        mdcLeistungen),
                                    new XElement("or", new XAttribute("abstract", true), new XAttribute("mandatory", true),
                                        new XAttribute("name", "LeistungenFab"),
                                        fabLeistungen)
                                ),
                                new XElement("and", new XAttribute("name", "ModifikationKrankenhaus"), new XAttribute("abstract", true),
                                    hospitalModifications)
                            )
                        )
                    ),
                    new XElement("constraints",
                        constraints
                    )
                )
            );

            return _xml;
        }

        private IEnumerable<XElement> FeaturesToXml(IList<IFeature> features, IList<XElement> constraints)
        {
            return (from feature in features
                let name = feature.Service.Type + feature.Service.Code
                let actualServices = ActualServicesToXml(feature, constraints)
                select new XElement("and", new XAttribute("name", name + "Global"),
                    AttributesToXml(name + "Global", feature.Attributes.ToList(), constraints), actualServices,
                    new XElement("feature", new XAttribute("name", name + "GlobalEinfrieren")))).ToList();
        }

        private IEnumerable<XElement> FabFeaturesToXml(IList<IFeature> features, IList<XElement> constraints)
        {
            var fabs = new List<IFab>();
            foreach (var feature in features)
            {
                foreach (var fab in feature.Service.Fabs)
                {
                    if (!fabs.Any(x => x.Name.Equals(fab.Name)))
                        fabs.Add(fab);
                }
            }

            return (from fab in fabs
                    let name = "FAB" + fab.Name
                    select new XElement("and", new XAttribute("name", name),
                        AttributesToXml(name, features.First().Attributes.ToList(), constraints), new XElement("feature",
                            new XAttribute("name", name + "Einfrieren"))))
                .ToList();
        }

        private XElement ActualServicesToXml(IFeature feature, IList<XElement> constraints)
        {
            var actualServicesXml = feature.Service.Fabs
                .Select(fab => feature.Service.Type + feature.Service.Code + "Standort0" + "Fachabteilung" + fab.Name).Select(name =>
                    new XElement("and", new XAttribute("name", name), AttributesToXml(name, feature.Attributes.ToList(), constraints),
                        new XElement("feature", new XAttribute("name", name + "Einfrieren"))))
                .ToList();

            return new XElement("or", new XAttribute("abstract", true), new XAttribute("mandatory", true),
                new XAttribute("name", feature.Service.Type + feature.Service.Code),
                actualServicesXml);
        }

        // private XElement AbstractServicesToXml(IFeature feature, IList<XElement> constraints)
        // {
        //     var abstractServicesXml = new List<XElement>();
        //
        //     //TODO: Notwendig, wenn Standort berücksichtigt wird
        //
        //     return new XElement("or", new XAttribute("abstract", true), new XAttribute("mandatory", true),
        //         new XAttribute("name", feature.Service.Type  + feature.Service.Code  + "Abstraktionsebenen"), abstractServicesXml);
        // }

        private XElement AttributesToXml(string name, IList<IAttribute> attributes, IList<XElement> constraints)
        {
            AddDefaultValueToAttributes(attributes);

            var attributesXml = attributes.Select(attribute => AttributeToXml(name, attribute, constraints)).ToList();

            return new XElement("alt", new XAttribute("abstract", true), new XAttribute("mandatory", true),
                new XAttribute("name", name + "Attributes"), attributesXml);
        }

        private void AddDefaultValueToAttributes(IList<IAttribute> attributes)
        {
            if (!attributes.Any(x => x.ValueScheme.Values.Contains(0))) return;

            var valueSchemesWithoutDefault = attributes.Where(x => !x.ValueScheme.Values.Contains(0)).Select(x => x.ValueScheme);
            foreach (var valueScheme in valueSchemesWithoutDefault)
            {
                valueScheme.Values.Add(0);
            }
        }

        private XElement AttributeToXml(string name, IAttribute attribute, IList<XElement> constraints)
        {
            var valueSchemeXml = ValueSchemeToXml(name, attribute, constraints);

            return new XElement("alt", new XAttribute("abstract", true),
                new XAttribute("name", name + attribute.Name), valueSchemeXml);
        }

        private IEnumerable<XElement> ValueSchemeToXml(string name, IAttribute attribute, IList<XElement> constraints)
        {
            foreach (var value in attribute.ValueScheme.Values)
            {
                if (value != 0)
                    constraints.Add(new XElement("rule",
                        new XElement("imp",
                            new XElement("var",
                                name + attribute.Name +
                                (value < 0 ? "Neg" + value.ToString(CultureInfo.InvariantCulture).Replace("-", String.Empty) : value)),
                            new XElement("not",
                                new XElement("var", name + "Einfrieren")))));
            }

            return attribute.ValueScheme.Values.Select(value => new XElement("feature",
                    new XAttribute("name",
                        name + attribute.Name +
                        (value < 0 ? "Neg" + value.ToString(CultureInfo.InvariantCulture).Replace("-", String.Empty) : value))))
                .ToList();
        }

        private IEnumerable<XElement> RandomXmlHospitalModifications()
        {
            var count = new Random().Next(1, 3);

            var hospitalModifications = new List<XElement>();

            for (var i = 0; i < count; i++)
            {
                var name = $"HospitalModification{i + 1}";
                hospitalModifications.Add(new XElement("alt", new XAttribute("mandatory", true),
                    new XAttribute("name", name),
                    new XElement("alt", new XAttribute("name", name + "ModAbsolut"),
                        new XElement("feature", new XAttribute("name", name + "ModAbsolut" + "50")),
                        new XElement("feature", new XAttribute("name", name + "ModAbsolut" + "Neg50"))),
                    new XElement("alt", new XAttribute("name", name + "ModPercent"),
                        new XElement("feature", new XAttribute("name", name + "ModPercent" + "50")),
                        new XElement("feature", new XAttribute("name", name + "ModPercent" + "Neg50")))));
            }

            return hospitalModifications;
        }

        private IList<XElement> FeaturesToConstraints(IEnumerable<IFeature> features)
        {
            var constraints = new List<XElement>();
            foreach (var feature in features)
            {
                var constraintsGlobalFreeze = new List<XElement>();

                var name = feature.Service.Type + feature.Service.Code;
                foreach (var fab in feature.Service.Fabs)
                {
                    constraints.Add(new XElement("rule",
                            new XElement("imp",
                                new XElement("var", name + "Standort0" + "Fachabteilung" + fab.Name),
                                new XElement("var", "FAB" + fab.Name)
                            )
                        )
                    );

                    constraintsGlobalFreeze.Add(new XElement("disj",
                        new XElement("not",
                            new XElement("var", name + "Standort0" + "Fachabteilung" + fab.Name)),
                        new XElement("var", name + "Standort0" + "Fachabteilung" + fab.Name + "Einfrieren")));
                }

                constraints.Add(new XElement("rule",
                    new XElement("eq",
                        new XElement("var", name + "GlobalEinfrieren"),
                        new XElement("conj", constraintsGlobalFreeze))));
            }

            return constraints;
        }
    }
}