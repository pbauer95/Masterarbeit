using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Masterarbeit.Classes.Logger
{
    public static class Logger
    {
        public static void StartLogEntry()
        {
            using StreamWriter w = File.AppendText("log.txt");
            w.WriteLine("=============================================");
            w.WriteLine("Log Entry Start");
        }

        public static void LogParameterValue(int partitionCount, IEnumerable<int> combinedPartitions, int maxSelectedFeatures)
        {
            var partitions = combinedPartitions as int[] ?? combinedPartitions.ToArray();
            using StreamWriter w = File.AppendText("log.txt");
            w.WriteLine($"Number of Partitions: {partitionCount}");
            w.WriteLine($"Number of combined Partitions: {partitions.Count()}");
            w.WriteLine($"Combined Partitions: {string.Join(",", partitions)}");
            w.WriteLine($"Max. Number of selected Features: {maxSelectedFeatures}");
        }

        public static void LogInitialFeatureCount(int initialFeatureCount)
        {
            using StreamWriter w = File.AppendText("log.txt");
            w.WriteLine($"Number of Initial Features: {initialFeatureCount}");
        }

        public static void LogSelectedFeaturesCount(int selectedFeaturesCount)
        {
            using StreamWriter w = File.AppendText("log.txt");
            w.WriteLine($"Number of selected Features: {selectedFeaturesCount}");
        }

        public static void LogFilledUpFeatures(int featureCount)
        {
            using StreamWriter w = File.AppendText("log.txt");
            w.WriteLine($"Number of added Features: {featureCount}");
        }

        public static void LogAttributeFeatures(int attributeFeaturesCount)
        {
            using StreamWriter w = File.AppendText("log.txt");
            w.WriteLine($"Number of Attribute Features: {attributeFeaturesCount}");
        }

        public static void LogTotalCountFeatures(int totalCount)
        {
            using StreamWriter w = File.AppendText("log.txt");
            w.WriteLine($"Total Number of Features: {totalCount}");
        }

        public static void LogDurationPartialFeatureSelection(TimeSpan timeSpan)
        {
            using StreamWriter w = File.AppendText("log.txt");
            w.WriteLine($"Duration generating partial Feature Selection: {Convert.ToInt32(timeSpan.TotalMilliseconds)} ms");
        }

        public static void LogDurationSampleGeneration(TimeSpan timeSpan)
        {
            using StreamWriter w = File.AppendText("log.txt");
            w.WriteLine($"Duration generating 2-wise Samples: {Convert.ToInt32(timeSpan.TotalMilliseconds)} ms");
        }
    }
}