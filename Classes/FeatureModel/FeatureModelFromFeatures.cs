using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Masterarbeit.Interfaces.Attribute;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.FeatureModel;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.FeatureModel
{
    public class FeatureModelFromFeatures : IFeatureModelXml
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

            var opsLeistungen =
                FeaturesToXml(Features.Where(x => x.Service?.Type == IService.ServiceType.Ops).ToList());
            var drgLeistungen =
                FeaturesToXml(Features.Where(x => x.Service?.Type == IService.ServiceType.Drg).ToList());
            var mlgLeistungen =
                FeaturesToXml(Features.Where(x => x.Service?.Type == IService.ServiceType.Mlg).ToList());
            var mdcLeistungen =
                FeaturesToXml(Features.Where(x => x.Service?.Type == IService.ServiceType.Mdc).ToList());
            var fabLeistungen =
                FeaturesToXml(Features.Where(x => x.Service?.Type == IService.ServiceType.Fab).ToList());

            //var hospitalModifications = RandomXmlHospitalModifications();

            _xml = new XDocument(
                new XDeclaration("1.0", "UTF-8", "no"),
                new XElement("featureModel",
                    new XElement("struct",
                        new XElement("and", new XAttribute("abstract", true), new XAttribute("mandatory", true),
                            new XAttribute("name", "PLATO"),
                            new XElement("and", new XAttribute("abstract", true), new XAttribute("mandatory", true),
                                new XAttribute("name", "Jahresplanung"),
                                new XElement("and", new XAttribute("abstract", true), new XAttribute("mandatory", true),
                                    new XAttribute("name", "Leistungen"),
                                    new XElement("or", new XAttribute("abstract", true),
                                        new XAttribute("mandatory", true),
                                        new XAttribute("name", "LeistungenOps"),
                                        opsLeistungen),
                                    new XElement("or", new XAttribute("abstract", true),
                                        new XAttribute("mandatory", true),
                                        new XAttribute("name", "LeistungenDrg"),
                                        drgLeistungen),
                                    new XElement("or", new XAttribute("abstract", true),
                                        new XAttribute("mandatory", true),
                                        new XAttribute("name", "LeistungenMlg"),
                                        mlgLeistungen),
                                    new XElement("or", new XAttribute("abstract", true),
                                        new XAttribute("mandatory", true),
                                        new XAttribute("name", "LeistungenMdc"),
                                        mdcLeistungen),
                                    new XElement("or", new XAttribute("abstract", true),
                                        new XAttribute("mandatory", true),
                                        new XAttribute("name", "LeistungenFab"),
                                        fabLeistungen)
                                )
                                // new XElement("and", new XAttribute("name", "ModifikationKrankenhaus"),
                                //     new XAttribute("abstract", true),
                                //     hospitalModifications)
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

        private IEnumerable<XElement> FeaturesToXml(IEnumerable<IFeature> features)
        {
            var groupedFeatures = features.GroupBy(x => x.Service.Code);

            var xelements = new List<XElement>();

            foreach (var group in groupedFeatures)
            {
                xelements.Add(GroupedFeaturesToXml(group.ToList()));
            }

            return xelements;
        }

        private XElement GroupedFeaturesToXml(IList<IFeature> features)
        {
            if (!features.Any(x => x.Global))
                throw new InvalidDataException();

            var globalFreezeXElement = FreezeXElement(features.Where(x => x.Global));

            var abstractionLevelXElement = AbstractionLevelXElement(features.Where(x => x.AbstractionLevel).ToList());
            var serviceProvisionsXElement =
                ServiceProvisionXElement(features.Where(x => !x.AbstractionLevel && !x.Global).ToList());

            var globalFeature = features.Single(x => x.Global && !x.Freeze);

            return new XElement("and", new XAttribute("name", globalFeature.Name), AttributesToXml(globalFeature),
                globalFreezeXElement, abstractionLevelXElement, serviceProvisionsXElement);
        }

        private XElement ServiceProvisionXElement(IList<IFeature> features)
        {
            if (!features.Any())
                return null;

            var serviceProvisionXElements = ServiceProvisionXElements(features.Where(x => x.Fab != null).ToList());

            var serviceProvisionsAbstractFeature = features.Single(x => x.Fab == null);
            return new XElement("or", new XAttribute("abstract", true), new XAttribute("mandatory", true),
                new XAttribute("name", serviceProvisionsAbstractFeature.Name), serviceProvisionXElements);
        }

        private IEnumerable<XElement> ServiceProvisionXElements(IList<IFeature> features)
        {
            if (!features.Any())
                throw new InvalidDataException();

            var fabGroupedServiceProvisionFeatures = features.GroupBy(x => x.Fab.Name);

            var serviceProvisionXElements = new List<XElement>();

            foreach (var fabGroup in fabGroupedServiceProvisionFeatures)
            {
                var freezeServiceProvisionXElement = FreezeXElement(fabGroup);

                var serviceProvisionFeature = fabGroup.Single(x => !x.Freeze);

                serviceProvisionXElements.Add(new XElement("and", new XAttribute("name", serviceProvisionFeature
                    .Name), freezeServiceProvisionXElement, AttributesToXml(serviceProvisionFeature)));
            }

            return serviceProvisionXElements;
        }

        private XElement AbstractionLevelXElement(IList<IFeature> features)
        {
            if (!features.Any(x => x.AbstractionLevel))
                return null;

            var abstractionLevelXElements = AbstractionLevelXElements(features.Where(x => x.Fab != null).ToList());

            var abstractionLevelAbstractFeature =
                features.Single(x => !x.Global && x.AbstractionLevel && x.Fab == null);

            return new XElement("or", new XAttribute("name", abstractionLevelAbstractFeature.Name), new XAttribute
                ("abstract", true), abstractionLevelXElements);
        }

        private IEnumerable<XElement> AbstractionLevelXElements(IList<IFeature> features)
        {
            if (!features.Any())
                throw new InvalidDataException();

            var fabGroupedAbstractionLevelFeatures = features.GroupBy(x => x.Fab.Name);

            var abstractionLevelXElements = new List<XElement>();

            foreach (var fabGroup in fabGroupedAbstractionLevelFeatures)
            {
                var freezeAbstractionLevelXElement = FreezeXElement(fabGroup);

                var abstractionLevelFeature = fabGroup.Single(x => !x.Freeze);

                abstractionLevelXElements.Add(new XElement("and",
                    new XAttribute("name", abstractionLevelFeature.Name), freezeAbstractionLevelXElement,
                    AttributesToXml(abstractionLevelFeature)));
            }

            return abstractionLevelXElements;
        }

        private XElement FreezeXElement(IEnumerable<IFeature> features)
        {
            var freezeFeature = features.SingleOrDefault(x => x.Freeze);
            return freezeFeature != null
                ? new XElement("feature", new XAttribute("name", freezeFeature.Name))
                : null;
        }

        private XElement AttributesToXml(IFeature feature)
        {
            if (feature.Attributes == null)
                return null;

            var attributesXml = feature.Attributes.Select(attribute => AttributeToXml(feature.Name, attribute))
                .ToList();

            return new XElement("alt", new XAttribute("abstract", true), new XAttribute("mandatory", true),
                new XAttribute("name", feature.Name + "Attributes"), attributesXml);
        }

        private XElement AttributeToXml(string name, IAttribute attribute)
        {
            var valueSchemeXml = ValueSchemeToXml(name, attribute);

            return new XElement("alt", new XAttribute("abstract", true),
                new XAttribute("name", name + attribute.Name), valueSchemeXml);
        }

        private IEnumerable<XElement> ValueSchemeToXml(string name, IAttribute attribute)
        {
            return attribute.ValueScheme.Values.Select(value => new XElement("feature",
                    new XAttribute("name",
                        name + attribute.Name +
                        (value < 0
                            ? "Neg" + value.ToString(CultureInfo.InvariantCulture).Replace("-", String.Empty)
                            : value))))
                .ToList();
        }

        // private IEnumerable<XElement> RandomXmlHospitalModifications()
        // {
        //     var count = new Random().Next(1, 3);
        //
        //     var hospitalModifications = new List<XElement>();
        //
        //     for (var i = 0; i < count; i++)
        //     {
        //         var name = $"HospitalModification{i + 1}";
        //         hospitalModifications.Add(new XElement("alt", new XAttribute("mandatory", true),
        //             new XAttribute("abstract", true),
        //             new XAttribute("name", name),
        //             new XElement("alt", new XAttribute("name", name + "ModAbsolut"), new XAttribute("abstract", true),
        //                 new XElement("feature", new XAttribute("name", name + "ModAbsolut" + "50")),
        //                 new XElement("feature", new XAttribute("name", name + "ModAbsolut" + "Neg50"))),
        //             new XElement("alt", new XAttribute("name", name + "ModPercent"), new XAttribute("abstract", true),
        //                 new XElement("feature", new XAttribute("name", name + "ModPercent" + "50")),
        //                 new XElement("feature", new XAttribute("name", name + "ModPercent" + "Neg50")))));
        //     }
        //
        //     return hospitalModifications;
        // }

        private IEnumerable<XElement> FeaturesToConstraints(IEnumerable<IFeature> features)
        {
            var featureList = features.ToList();

            var featureDictionary = featureList.Where(x => x.Service != null).GroupBy(x => (x.Service
                .Type, x.Service.Code)).ToDictionary(x => x.Key, x => x.ToList());

            var constraints = new List<XElement>();
            foreach (var feature in featureList)
            {
                if (feature.Global && feature.Freeze && feature.Service?.Type != IService.ServiceType.Fab)
                    AddGlobalFreezeConstraints(feature, constraints, featureDictionary);

                if (feature.AbstractionLevel && feature.Freeze)
                    AddAbstractionLevelFreezeConstraints(feature, constraints, featureDictionary);

                if (feature.AbstractionLevel && !feature.Freeze && feature.Fab != null)
                    AddAbstractionLevelProvideConstraints(feature, constraints, featureDictionary);

                if (feature.Attributes != null)
                    AddAttributeConstraints(feature, constraints, featureDictionary);

                if (feature.Service?.Type == IService.ServiceType.Fab)
                    AddFabConstraints(feature, constraints);
            }

            return constraints;
        }

        private void AddGlobalFreezeConstraints(IFeature constrainedFeature, ICollection<XElement> constraints,
            IDictionary<(IService.ServiceType, string), List<IFeature>> featureDictionary)
        {
            var featuresInConstraint =
                featureDictionary[(constrainedFeature.Service.Type, constrainedFeature.Service.Code)]
                    .Where(x => x.Freeze && x.AbstractionLevel);

            var abstractionLevelsGlobalFreezeConstraint = AbstractionLevelsGlobalFreezeConstraint(featuresInConstraint);

            constraints.Add(new XElement("rule", new XElement("eq", new XElement("var", constrainedFeature.Name),
                new XElement("conj", abstractionLevelsGlobalFreezeConstraint))));
        }

        private IEnumerable<XElement> AbstractionLevelsGlobalFreezeConstraint(IEnumerable<IFeature> features)
        {
            return features.Select(feature => new XElement("disj",
                new XElement("not", new XElement("var", feature.Name.Replace("Freeze", ""))),
                new XElement("var", feature.Name))).ToList();
        }

        private void AddAbstractionLevelFreezeConstraints(IFeature constrainedFeature,
            ICollection<XElement> constraints,
            IDictionary<(IService.ServiceType, string), List<IFeature>> featureDictionary)
        {
            var featuresInConstraint =
                featureDictionary[(constrainedFeature.Service.Type, constrainedFeature.Service.Code)]
                    .Where(x => x.Fab == constrainedFeature.Fab && !x.AbstractionLevel && !x.Global);

            var abstractionLevelsServiceProvisionFreezeConstraint =
                AbstractionLevelsServiceProvisionFreezeConstraint(featuresInConstraint);

            constraints.Add(new XElement("rule",
                new XElement("eq", new XElement("var", constrainedFeature.Name),
                    abstractionLevelsServiceProvisionFreezeConstraint)));
        }

        private void AddAbstractionLevelProvideConstraints(IFeature constrainedFeature,
            ICollection<XElement> constraints,
            IDictionary<(IService.ServiceType, string), List<IFeature>> featureDictionary)
        {
            var featureInConstraint = featureDictionary[(constrainedFeature.Service.Type, constrainedFeature.Service
                .Code)].Single(x => x.Fab == constrainedFeature.Fab && !x.Freeze && !x.AbstractionLevel && x.Attributes
                != null);

            constraints.Add(new XElement("rule",
                new XElement("eq", new XElement("var", constrainedFeature.Name),
                    new XElement("var", featureInConstraint.Name))));
        }

        private XElement AbstractionLevelsServiceProvisionFreezeConstraint(IEnumerable<IFeature> features)
        {
            var featureList = features.ToList();
            var frozenFeature = featureList.Single(x => x.Freeze);
            var correspondingFeature = featureList.Single(x => !x.Freeze);

            return new XElement("disj", new XElement("not", new XElement("var", correspondingFeature.Name)),
                new XElement("var", frozenFeature.Name));
        }

        private void AddAttributeConstraints(IFeature constrainedFeature, ICollection<XElement> constraints,
            IDictionary<(IService.ServiceType, string), List<IFeature>> featureDictionary)
        {
            var featureInConstraint =
                featureDictionary[(constrainedFeature.Service.Type, constrainedFeature.Service.Code)]
                    .SingleOrDefault(x =>
                        x.Fab == constrainedFeature.Fab && x.AbstractionLevel == constrainedFeature.AbstractionLevel
                                                        && x.Global == constrainedFeature.Global && x.Freeze);

            if (featureInConstraint == null)
                return;

            var freezeAttributesConstraint =
                FreezeAttributesConstraint(constrainedFeature, constrainedFeature.Attributes);

            constraints.Add(new XElement("rule", new XElement("var", featureInConstraint.Name),
                new XElement("conj", freezeAttributesConstraint)));
        }

        private IEnumerable<XElement> FreezeAttributesConstraint(IFeature feature, IEnumerable<IAttribute> attributes)
        {
            return (from attribute in attributes
                from value in attribute.ValueScheme.Values
                where value != 0
                select new XElement("not", new XElement("var", feature.Name + attribute.Name + (value < 0
                    ? "Neg" + value.ToString(CultureInfo.InvariantCulture).Replace("-", string.Empty)
                    : value)))).ToList();
        }

        private void AddFabConstraints(IFeature constrainedFeature, ICollection<XElement> constraints)
        {
            if (constrainedFeature.Freeze)
            {
                AddFabFreezeConstraints(constrainedFeature, constraints);
            }
            else
            {
                AddFabProvideConstraints(constrainedFeature, constraints);
            }
        }

        private void AddFabFreezeConstraints(IFeature constrainedFeature, ICollection<XElement> constraints)
        {
            var featuresInConstraint = Features.Where(x =>
                    x.Fab?.Name == constrainedFeature.Fab?.Name && x.Freeze &&
                    x.Service?.Type != IService.ServiceType.Fab)
                .ToList();

            if (!featuresInConstraint.Any())
                return;

            var fabFreezeConstraints = FabFreezeConstraints(featuresInConstraint);

            constraints.Add(new XElement("rule",
                new XElement("imp", new XElement("var", constrainedFeature.Name),
                    new XElement("conj", fabFreezeConstraints))));
        }

        private IEnumerable<XElement> FabFreezeConstraints(IEnumerable<IFeature> features)
        {
            return features.Select(feature => new XElement("var", feature.Name)).ToList();
        }

        private void AddFabProvideConstraints(IFeature constrainedFeature, ICollection<XElement> constraints)
        {
            var featuresInConstraint = Features.Where(x =>
                    x.Fab?.Name == constrainedFeature.Fab?.Name && !x.Freeze &&
                    x.Service?.Type != IService.ServiceType.Fab)
                .ToList();

            var fabProvideConstraints = FabProvideConstraints(featuresInConstraint);

            constraints.Add(new XElement("rule",
                new XElement("imp", new XElement("not", new XElement("var", constrainedFeature.Name)),
                    new XElement("conj", fabProvideConstraints))));
        }

        private IEnumerable<XElement> FabProvideConstraints(IEnumerable<IFeature> features)
        {
            return features.Select(feature => new XElement("not", new XElement("var", feature.Name))).ToList();
        }

        // var constraintsGlobalFreeze = new List<XElement>();
        //
        // var name = feature.Service.Type + feature.Service.Code;
        // foreach (var fab in feature.Service.Fabs)
        // {
        //     constraints.Add(new XElement("rule",
        //             ,
        //                 new XElement("var", name + "Standort0" + "Fachabteilung" + fab.Name),
        //                 new XElement("var", "FAB" + fab.Name)
        //             )
        //         )
        //     );
        //
        //     constraintsGlobalFreeze.Add(new XElement("disj",
        //         new XElement("not",
        //             new XElement("var", name + "Standort0" + "Fachabteilung" + fab.Name)),
        //         new XElement("var", name + "Standort0" + "Fachabteilung" + fab.Name + "Einfrieren")));
        // }
        //
        // constraints.Add(new XElement("rule",
        //     new XElement("eq",
        //         new XElement("var", name + "GlobalEinfrieren"),
        //         new XElement("conj", constraintsGlobalFreeze))));
    }
}