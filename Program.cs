using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using Masterarbeit.Interfaces.DistributionData;
using Masterarbeit.Interfaces.FeatureModel;
using Masterarbeit.Interfaces.Partition;

namespace Masterarbeit
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<CommandLineOptions>(args)
                .MapResult(options =>
                {
                    try
                    {
                        var statistic = new StatisticFromXml(options.StatisticPath);

                        var featureModel = GenerateFeatureModel(options, statistic);

                        Logger.StartLogEntry(featureModel, options);

                        while (!IsSufficientlyReduced(featureModel, 10000))
                        {
                            featureModel = ReducedFeatureModel(featureModel, statistic, options.PartitionCount,
                                options.CombinedPartition, options.MaxSelectedFeatures);
                        }

                        Logger.LogReducedFeatureModelStats(featureModel);

                        if (options.GenerateSample)
                            GenerateSample((featureModel as IFeatureModelXml)?.ToXml(), options.Interactions);

                        Logger.WriteLogEntry();

                        return 0;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine(e.ToString());
                        return -3;
                    }
                }, _ => -1);
        }

        private static IFeatureModel ReducedFeatureModel(IFeatureModel featureModel, IStatistic statistic, int
            partitionCount, IEnumerable<int> combinedPartitions, int maxSelectedFeatures)
        {
            var features = featureModel.Features.ToList();

            var classifiedFeatures =
                new ClassifiedFeatures(features, statistic).ToList();

            Logger.LogPartitionStart();
            IPartitionData partitionData =
                new PartitionDataFromFeatures(classifiedFeatures, partitionCount);
            Logger.LogPartitionEnd();

            Logger.LogSelectionStart();
            var selectedFeatures =
                partitionData.SelectFeaturesFromPartitions(combinedPartitions, maxSelectedFeatures).ToList();
            Logger.LogSelectionEnd();

            Logger.LogMissingFeaturesStart();
            var missingRequiredFeatures =
                new MissingRequiredFeatures(selectedFeatures, featureModel).ToList();
            Logger.LogMissingFeaturesEnd();

            var combinedFeatures = selectedFeatures.Concat(missingRequiredFeatures).ToList();

            return new FeatureModelFromFeatures(combinedFeatures);
        }

        private static IFeatureModel GenerateFeatureModel(CommandLineOptions options, StatisticFromXml statistic)
        {
            var attributes = new AttributesFromXml(options.AttributesPath);

            IFeatureModel unreducedFeatureModel = options.HospitalDatabasePath == "-1"
                ? new FeatureModelFromStatistic(statistic, attributes)
                : new FeatureModelFromHospitalDatabase(
                    new HospitalDatabaseFromXml(options.HospitalDatabasePath),
                    attributes);

            return unreducedFeatureModel;
        }

        private static bool IsSufficientlyReduced(IFeatureModel featureModel, int maxSize)
        {
            var featureList = featureModel.Features.ToList();
            var totalCount = featureList.Count + featureList.Count(x => x.Attributes != null) * 9;
            return totalCount <= maxSize;
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

            var myProcess = new Process();
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.FileName = "java";
            myProcess.StartInfo.Arguments =
                $"-jar {generatorPath} genconfig -a YASA -t {tWise} -fm {fmFileName} -o {outputPath}";

            Logger.LogSampleStart();
            myProcess.Start();

            myProcess.WaitForExit(14400000);
            Logger.LogSampleEnd();

            if (!File.Exists(@".\sample.csv")) 
                return;
            
            var lines = File.ReadAllLines(@".\sample.csv");
            Logger.LogSampleSize(lines.Length - 1);
            
            File.Delete(@".\sample.csv");
        }
    }
}