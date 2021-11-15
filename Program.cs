using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using CommandLine;
using Masterarbeit.Classes.Attribute;
using Masterarbeit.Classes.CommandLineOptions;
using Masterarbeit.Classes.DistributionData;
using Masterarbeit.Classes.Feature;
using Masterarbeit.Classes.FeatureModel;
using Masterarbeit.Classes.HospitalData;
using Masterarbeit.Classes.Logger;
using Masterarbeit.Classes.Partition;
using Masterarbeit.Interfaces.Fab;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.Partition;

namespace Masterarbeit
{
    class Program
    {
        private const int CoreFeatures = 9;

        private static DateTime _startTime;

        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<CommandLineOptions>(args)
                .MapResult(options =>
                {
                    try
                    {
                        _startTime = DateTime.UtcNow;

                        Logger.StartLogEntry();
                        Logger.LogParameterValue(options.PartitionCount, options.CombinedPartition, options.MaxSelectedFeatures);

                        var masterData = new DistributionDataFromXml(options.DistributionDataPath);

                        Logger.LogInitialFeatureCount(masterData.Services.Sum(x => x.MasterDataFabs.Count()));

                        IPartitionData partitionData = options.HospitalData == "-1"
                            ? new PartitionDataFromMasterData(masterData, options.PartitionCount)
                            : new PartitionDataFromHospitalData(new HospitalDataFromXml(options.HospitalData), masterData, options.PartitionCount);

                        var selectedFeatures =
                            partitionData.SelectFeaturesFromPartitions(options.CombinedPartition, options.MaxSelectedFeatures,
                                options.Global).ToList();

                        var selectedFeatureCount = selectedFeatures.Sum(x => x.Service.Fabs.Count());
                        Logger.LogSelectedFeaturesCount(selectedFeatureCount);

                        var filledUpFeatureCount = CoreFeatures + selectedFeatures.Count * 3 +
                                                   selectedFeatures.Sum(x => x.Service.Fabs.Count()) + FabsCount(selectedFeatures) * 2;
                        Logger.LogFilledUpFeatures(filledUpFeatureCount);

                        var featuresWithAttributes =
                            new FeaturesWithFabsAndAttributes(selectedFeatures, new AttributesFromXml(options.AttributeDataPath));

                        var attributeFeatureCount = selectedFeatures.Count * 9 + selectedFeatures.Sum(x => x.Service.Fabs.Count()) * 9 +
                                                    FabsCount(selectedFeatures) * 9;
                        Logger.LogAttributeFeatures(attributeFeatureCount);

                        var featureModel = new FeatureModelFromFeatures(featuresWithAttributes);

                        Logger.LogTotalCountFeatures(selectedFeatureCount + filledUpFeatureCount + attributeFeatureCount);

                        GenerateSample(featureModel.ToXml(), options.Interactions);

                        return 0;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return -3;
                    }
                }, errs => -1);
        }

        private static void GenerateSample(XDocument featureDiagram, int tWise)
        {
            const string fmFileName = "FeatureModel.xml";
            const string generatorPath = "de.ovgu.featureide.lib.fm.jar";
            const string outputPath = "sample.csv";

            using (var writer = new XmlTextWriter(fmFileName, new UTF8Encoding(false)))
            {
                featureDiagram.Save(writer);
            }

            Logger.LogDurationPartialFeatureSelection(DateTime.UtcNow - _startTime);

            var myProcess = new Process();
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.FileName = "java";
            myProcess.StartInfo.Arguments =
                $"-jar {generatorPath} genconfig -a YASA -t {tWise} -fm {fmFileName} -o {outputPath}";

            var startTimeSampleGeneration = DateTime.UtcNow;

            myProcess.Start();

            myProcess.WaitForExit();

            Logger.LogDurationSampleGeneration(DateTime.UtcNow - startTimeSampleGeneration);
        }

        private static int FabsCount(IEnumerable<IFeature> features)
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

            return fabs.Count;
        }
    }
}