using System;
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
using Masterarbeit.Interfaces.FeatureModel;
using Masterarbeit.Interfaces.Partition;

namespace Masterarbeit
{
    internal static class Program
    {
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
                        Logger.LogParameterValue(options);

                        var statistic = new StatisticFromXml(options.StatisticPath);

                        var attributes = new AttributesFromXml(options.AttributesPath);

                        IFeatureModel unreducedFeatureModel = options.HospitalDatabasePath == "-1"
                            ? new FeatureModelFromStatistic(statistic, attributes)
                            : new FeatureModelFromHospitalDatabase(new HospitalDatabaseFromXml(options.HospitalDatabasePath),
                                attributes);

                        Logger.LogInitialFeatureCount(unreducedFeatureModel.Features.Count());

                        var classifiedFeatures =
                            new ClassifiedFeatures(unreducedFeatureModel.Features, statistic);

                        IPartitionData partitionData =
                            new PartitionDataFromFeatures(classifiedFeatures, options.PartitionCount);

                        var selectedFeatures =
                            partitionData.SelectFeaturesFromPartitions(options.CombinedPartition.ToList(), options
                                .MaxSelectedFeatures).ToList();

                        Logger.LogSelectedFeaturesCount(selectedFeatures.Count);

                        var missingRequiredFeatures =
                            new MissingRequiredFeatures(selectedFeatures, unreducedFeatureModel).ToList();

                        Logger.LogFilledUpFeatures(missingRequiredFeatures.Count);

                        var combinedFeatures = selectedFeatures.Concat(missingRequiredFeatures).ToList();

                        Logger.LogTotalCountFeatures(combinedFeatures);
                        Logger.LogAttributeFeatures(combinedFeatures);

                        var featureModel = new FeatureModelFromFeatures(combinedFeatures);

                        GenerateSample(featureModel.ToXml(), options.Interactions);

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
    }
}