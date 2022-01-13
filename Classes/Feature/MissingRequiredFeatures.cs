using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Masterarbeit.Classes.FeatureModel;
using Masterarbeit.Classes.Service;
using Masterarbeit.Interfaces.Fab;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.FeatureModel;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.Feature
{
    public class MissingRequiredFeatures : IEnumerable<IFeature>
    {
        private readonly IEnumerable<IFeature> _selectedFeatures;
        private readonly IDictionary<(IService.ServiceType, string), List<IFeature>> _unreducedFeatures;
        private readonly IEnumerable<IFeature> _mandatoryFeatures;
        private IList<IFeature> _missingRequiredFeatures;

        public MissingRequiredFeatures(IEnumerable<IFeature> selectedFeatures, IFeatureModel featureModel)
        {
            _selectedFeatures = selectedFeatures;
            _mandatoryFeatures = featureModel.Features.Where(x => x.Mandatory);
            _unreducedFeatures = featureModel.Features.Where(x => x.Service != null).GroupBy(x => (x.Service
                .Type, x.Service.Code)).ToDictionary(x => x.Key, x => x.ToList());
        }

        public IEnumerator<IFeature> GetEnumerator() => DeterminedMissingRequiredFeatures().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<IFeature> DeterminedMissingRequiredFeatures()
        {
            if (_missingRequiredFeatures != null)
                return _missingRequiredFeatures;

            var missingRequiredFeatures = new List<IFeature>();

            foreach (var feature in _selectedFeatures)
            {
                AddMissingFeaturesForFeature(feature, missingRequiredFeatures);
            }

            AddMissingFabFeatures(missingRequiredFeatures);

            missingRequiredFeatures.AddRange(_mandatoryFeatures);

            return _missingRequiredFeatures = missingRequiredFeatures;
        }

        private void AddMissingFeaturesForFeature(IFeature feature, ICollection<IFeature> filledUpFeatures)
        {
            AddGlobalMissingFeatures(feature, filledUpFeatures);
            AddAbstractionLevelMissingFeatures(feature, filledUpFeatures);
            AddConcreteMissingFeatures(feature, filledUpFeatures);
        }

        private void AddGlobalMissingFeatures(IFeature feature, ICollection<IFeature> filledUpFeatures)
        {
            if (!feature.Global)
                return;

            if (!feature.Freeze)
            {
                AddMissingFeaturesForGlobalFeature(feature, filledUpFeatures);
            }
            else
            {
                AddMissingFeaturesForGlobalFreezeFeature(feature, filledUpFeatures);
            }
        }

        private void AddAbstractionLevelMissingFeatures(IFeature feature, ICollection<IFeature> filledUpFeatures)
        {
            if (!feature.AbstractionLevel)
                return;

            AddMissingAbstractionLevelsFeature(feature, filledUpFeatures);

            if (!feature.Freeze)
            {
                AddMissingFeaturesForAbstractionLevelFeature(feature, filledUpFeatures);
            }
            else
            {
                AddMissingAbstractionLevelFeature(feature, filledUpFeatures);
                AddMissingFeatureForAbstractionLevelFreezeFeature(feature, filledUpFeatures);
            }
        }

        private void AddConcreteMissingFeatures(IFeature feature, ICollection<IFeature> filledUpFeatures)
        {
            if (feature.Global || feature.AbstractionLevel)
                return;

            if (!feature.Freeze)
            {
                AddMissingFeaturesForConcreteServiceProvisionFeature(feature, filledUpFeatures);
            }
            else
            {
                AddMissingFeaturesForConcreteFreezeFeature(feature, filledUpFeatures);
            }
        }

        private void AddMissingFeaturesForConcreteFreezeFeature(IFeature feature,
            ICollection<IFeature> filledUpFeatures)
        {
            if (_selectedFeatures.Any(x =>
                    IsSameMedicalService(x, feature) && !x.AbstractionLevel && !x.Global && !x.Freeze &&
                    IsSameFab(x, feature)))
                return;

            if (filledUpFeatures.Any(x =>
                    IsSameMedicalService(x, feature) && !x.AbstractionLevel && !x.Global && !x.Freeze &&
                    IsSameFab(x, feature)))
                return;

            var fillUpFeature = _unreducedFeatures[(feature.Service.Type, feature.Service.Code)]
                .Single(x => !x.AbstractionLevel && !x.Global && !x.Freeze && x.Fab == feature.Fab);

            filledUpFeatures.Add(fillUpFeature);
            AddConcreteMissingFeatures(fillUpFeature, filledUpFeatures);
        }

        private void AddMissingFeaturesForConcreteServiceProvisionFeature(IFeature feature,
            ICollection<IFeature> filledUpFeatures)
        {
            AddMissingAbstractFeatureForConcreteServiceProvision(feature, filledUpFeatures);
            AddMissingGlobalFeatureForConcreteServiceProvision(feature, filledUpFeatures);
        }

        private void AddMissingGlobalFeatureForConcreteServiceProvision(IFeature feature,
            ICollection<IFeature> filledUpFeatures)
        {
            if (_selectedFeatures.Any(x => IsSameMedicalService(x, feature) && x.Global && !x.Freeze))
                return;

            if (!filledUpFeatures.Any(x => IsSameMedicalService(x, feature) && x.Global && !x.Freeze))
            {
                filledUpFeatures.Add(_unreducedFeatures[(feature.Service.Type, feature.Service.Code)]
                    .Single(x => x.Global && !x.Freeze));
            }
        }

        private void AddMissingAbstractFeatureForConcreteServiceProvision(IFeature feature,
            ICollection<IFeature> filledUpFeatures)
        {
            if (_selectedFeatures.Any(x =>
                    IsSameMedicalService(x, feature) && !x.AbstractionLevel && !x.Global && x.Fab == null))
                return;

            if (!filledUpFeatures.Any(x =>
                    IsSameMedicalService(x, feature) && !x.AbstractionLevel && !x.Global && x.Fab == null))
            {
                filledUpFeatures.Add(_unreducedFeatures[(feature.Service.Type, feature.Service.Code)]
                    .Single(x => !x.AbstractionLevel && !x.Global && x.Fab == null));
            }
        }

        private void AddMissingFeaturesForGlobalFeature(IFeature feature, ICollection<IFeature> filledUpFeatures)
        {
            if (_selectedFeatures.Any(x =>
                    IsSameMedicalService(x, feature) && !x.AbstractionLevel && !x.Global && x.Fab != null))
                return;

            if (filledUpFeatures.Any(x =>
                    IsSameMedicalService(x, feature) && !x.AbstractionLevel && !x.Global && x.Fab != null))
                return;

            var unreducedFeatures = _unreducedFeatures[(feature.Service.Type, feature.Service.Code)]
                .Where(x => !x.AbstractionLevel && !x.Freeze).ToList();

            filledUpFeatures.Add(unreducedFeatures.Single(x => x.Fab == null && !x.Global));
            filledUpFeatures.Add(unreducedFeatures.First(x => x.Fab != null));
        }

        private void AddMissingFeaturesForGlobalFreezeFeature(IFeature feature, ICollection<IFeature> filledUpFeatures)
        {
            if (!filledUpFeatures.Any(x => IsSameMedicalService(x, feature) && x.Global && !x.Freeze))
            {
                var globalFeature =
                    _selectedFeatures.SingleOrDefault(x => IsSameMedicalService(x, feature) && x.Global && !x.Freeze);

                if (globalFeature == null)
                {
                    globalFeature = _unreducedFeatures[(feature.Service.Type, feature.Service.Code)]
                        .Single(x => IsSameMedicalService(x, feature) && x.Global && !x.Freeze);

                    filledUpFeatures.Add(globalFeature);
                }

                AddGlobalMissingFeatures(globalFeature, filledUpFeatures);
            }

            var concreteServiceFeatures = _selectedFeatures
                .Where(x => IsSameMedicalService(x, feature) && x.Fab != null)
                .Concat(filledUpFeatures.Where(x => IsSameMedicalService(x, feature) && x.Fab != null));


            var abstractionLevelFabs = concreteServiceFeatures.Select(x => x.Fab.Name).Distinct().ToList();

            foreach (var abstractionLevelFab in abstractionLevelFabs)
            {
                if (_selectedFeatures.Any(x =>
                        IsSameMedicalService(x, feature) && x.AbstractionLevel && x.Freeze &&
                        x.Fab?.Name == abstractionLevelFab) ||
                    filledUpFeatures.Any(x =>
                        IsSameMedicalService(x, feature) && x.AbstractionLevel && x.Freeze &&
                        x.Fab?.Name == abstractionLevelFab))
                    continue;

                var fillUpFeature = _unreducedFeatures[(feature.Service.Type, feature.Service.Code)].Single(x =>
                    x.AbstractionLevel && x.Freeze && x.Fab.Name == abstractionLevelFab);

                filledUpFeatures.Add(fillUpFeature);
                AddAbstractionLevelMissingFeatures(fillUpFeature, filledUpFeatures);
            }
        }

        private void AddMissingAbstractionLevelsFeature(IFeature feature, ICollection<IFeature> filledUpFeatures)
        {
            if (_selectedFeatures.Any(x =>
                    IsSameMedicalService(x, feature) && x.AbstractionLevel && !x.Global && x.Fab == null))
                return;

            if (!filledUpFeatures.Any(x =>
                    IsSameMedicalService(x, feature) && x.AbstractionLevel && !x.Global && x.Fab == null))
            {
                filledUpFeatures.Add(_unreducedFeatures[(feature.Service.Type, feature.Service.Code)]
                    .Single(x => x.AbstractionLevel && !x.Global && x.Fab == null));
            }
        }

        private void AddMissingFeatureForAbstractionLevelFreezeFeature(IFeature feature,
            ICollection<IFeature> filledUpFeatures)
        {
            if (_selectedFeatures.Any(x =>
                    IsSameMedicalService(x, feature) && !x.AbstractionLevel && IsSameFab(x, feature) &&
                    x.Freeze))
                return;

            if (filledUpFeatures.Any(x =>
                    IsSameMedicalService(x, feature) && !x.AbstractionLevel && IsSameFab(x, feature) &&
                    x.Freeze))
                return;

            var fillUpFeature = _unreducedFeatures[(feature.Service.Type, feature.Service.Code)]
                .Single(x => !x.AbstractionLevel && IsSameFab(x, feature) && x.Freeze);

            filledUpFeatures.Add(fillUpFeature);
            AddConcreteMissingFeatures(fillUpFeature, filledUpFeatures);
        }

        private void AddMissingAbstractionLevelFeature(IFeature feature, ICollection<IFeature> filledUpFeatures)
        {
            if (_selectedFeatures.Any(x =>
                    IsSameMedicalService(x, feature) && x.AbstractionLevel && IsSameFab(x, feature) &&
                    !x.Freeze))
                return;

            if (filledUpFeatures.Any(x =>
                    IsSameMedicalService(x, feature) && x.AbstractionLevel && IsSameFab(x, feature) &&
                    !x.Freeze))
                return;

            var fillUpFeature = _unreducedFeatures[(feature.Service.Type, feature.Service.Code)]
                .Single(x => x.AbstractionLevel && IsSameFab(x, feature) && !x.Freeze);

            filledUpFeatures.Add(fillUpFeature);

            AddAbstractionLevelMissingFeatures(fillUpFeature, filledUpFeatures);
        }

        private void AddMissingFeaturesForAbstractionLevelFeature(IFeature feature,
            ICollection<IFeature> filledUpFeatures)
        {
            if (_selectedFeatures.Any(x =>
                    IsSameMedicalService(x, feature) && IsSameFab(x, feature) && !x.AbstractionLevel && !x.Freeze))
                return;

            if (filledUpFeatures.Any(x =>
                    IsSameMedicalService(x, feature) && IsSameFab(x, feature) && !x.AbstractionLevel && !x.Freeze))
                return;

            var fillUpFeature = _unreducedFeatures[(feature.Service.Type, feature.Service.Code)]
                .Single(x => !x.AbstractionLevel && !x.Global && IsSameFab(x, feature) && !x.Freeze);

            filledUpFeatures.Add(fillUpFeature);
            AddConcreteMissingFeatures(fillUpFeature, filledUpFeatures);
        }

        private void AddMissingFabFeatures(ICollection<IFeature> filledUpFeatures)
        {
            var missingFabs = MissingFabs(filledUpFeatures);

            var attributes = _selectedFeatures.First(x => x.Attributes != null).Attributes.ToList();

            foreach (var missingFab in missingFabs)
            {
                var fab = new Fab.Fab(missingFab.Key, missingFab.Value);

                var fabService = new FabService
                {
                    Code = fab.Name,
                    Fabs = new List<IFab> { fab }
                };


                filledUpFeatures.Add(new Feature
                {
                    Service = fabService,
                    Fab = fab,
                    Attributes = attributes,
                    Global = true,
                    CaseMix = fab.CaseMix
                });

                filledUpFeatures.Add(new Feature
                {
                    Service = fabService,
                    Fab = fab,
                    Global = true,
                    CaseMix = fab.CaseMix,
                    Freeze = true
                });
            }
        }

        private Dictionary<string, decimal> MissingFabs(ICollection<IFeature> filledUpFeatures)
        {
            var features = filledUpFeatures.Concat(_selectedFeatures).ToList();

            var missingFabs = new Dictionary<string, decimal>();
            foreach (var feature in features.Where(x => x.Service != null && x.Fab != null))
            {
                if (missingFabs.ContainsKey(feature.Fab.Name))
                    missingFabs[feature.Fab.Name] += feature.Fab.CaseMix;
                else
                {
                    missingFabs.Add(feature.Fab.Name, feature.Fab.CaseMix);
                }
            }

            return missingFabs;
        }

        private bool IsSameMedicalService(IFeature feature1, IFeature feature2)
        {
            return feature1.Service.Type == feature2.Service.Type && feature1.Service.Code == feature2.Service.Code;
        }

        private bool IsSameFab(IFeature feature1, IFeature feature2)
        {
            if (feature1.Fab == null || feature2.Fab == null)
                return false;

            return feature1.Fab.Name == feature2.Fab.Name;
        }
    }
}