using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Masterarbeit.Interfaces.Feature;

namespace Masterarbeit.Classes.Logger
{
    public static class Logger
    {
        public static void StartLogEntry()
        {
            using var w = File.AppendText("log.txt");
            w.WriteLine("=============================================");
            w.WriteLine("Log Entry Start");
        }

        public static void LogParameterValue(CommandLineOptions.CommandLineOptions options)
        {
            var partitions = options.CombinedPartition as int[] ?? options.CombinedPartition.ToArray();
            using var w = File.AppendText("log.txt");
            w.WriteLine($"Number of Partitions: {options.PartitionCount}");
            w.WriteLine($"Number of combined Partitions: {partitions.Length}");
            w.WriteLine($"Combined Partitions: {string.Join(",", partitions)}");
            w.WriteLine($"Max. Number of selected Features: {options.MaxSelectedFeatures}");
        }

        public static void LogInitialFeatureCount(int initialFeatureCount)
        {
            using var w = File.AppendText("log.txt");
            w.WriteLine($"Number of Initial Features: {initialFeatureCount}");
        }

        public static void LogSelectedFeaturesCount(int selectedFeaturesCount)
        {
            using var w = File.AppendText("log.txt");
            w.WriteLine($"Number of selected Features: {selectedFeaturesCount}");
        }

        public static void LogFilledUpFeatures(int featureCount)
        {
            using var w = File.AppendText("log.txt");
            w.WriteLine($"Number of added Features: {featureCount}");
        }

        public static void LogAttributeFeatures(IEnumerable<IFeature> features)
        {
            var attributeFeaturesCount = features.Count(x => x.Attributes != null);

            using var w = File.AppendText("log.txt");
            w.WriteLine($"Number of Attribute Features: {(attributeFeaturesCount - 90) * 9}");
        }

        public static void LogTotalCountFeatures(IEnumerable<IFeature> features)
        {
            using var w = File.AppendText("log.txt");
            w.WriteLine($"Total Number of Features: {features.Count()}");
        }

        public static void LogDurationPartialFeatureSelection(TimeSpan timeSpan)
        {
            using var w = File.AppendText("log.txt");
            w.WriteLine(
                $"Duration generating partial Feature Selection: {Convert.ToInt32(timeSpan.TotalMilliseconds)} ms");
        }

        public static void LogDurationSampleGeneration(TimeSpan timeSpan)
        {
            using var w = File.AppendText("log.txt");
            w.WriteLine($"Duration generating 2-wise Samples: {Convert.ToInt32(timeSpan.TotalMilliseconds)} ms");
        }
    }
}