using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Masterarbeit.Interfaces.Feature;

namespace Masterarbeit.Classes.Logger
{
    public static class Logger
    {
        private static LogEntry _logEntry;

        public static void StartLogEntry(CommandLineOptions.CommandLineOptions options)
        {
            _logEntry = new LogEntry();
            _logEntry.SetTotalStart();
            _logEntry.LogParameters(options);
        }

        public static void LogPartitionStart()
        {
            _logEntry.SetPartitionStart();
        }

        public static void LogInitialFeatureCount(IEnumerable<IFeature> features)
        {
            var featureList = features.ToList();
            _logEntry.InitialFeatureCount = featureList.Count + featureList.Count(x => x.Attributes != null) * 9;
        }

        public static void LogPartitionEnd()
        {
            _logEntry.SetPartitionEnd();
        }

        public static void LogSelectionStart()
        {
            _logEntry.SetSelectionStart();
        }

        public static void LogSelectionEnd()
        {
            _logEntry.SetSelectionEnd();
        }

        public static void LogMissingFeaturesStart()
        {
            _logEntry.SetMissingFeatureStart();
        }

        public static void LogMissingFeaturesEnd()
        {
            _logEntry.SetMissingFeatureEnd();
        }

        public static void LogSelectedFeaturesCount(int selectedFeaturesCount)
        {
            _logEntry.SelectedFeatureCount = selectedFeaturesCount;
        }

        public static void LogMissingFeatureCount(int missingFeatureCount)
        {
            _logEntry.MissingFeatureCount = missingFeatureCount;
        }

        public static void LogAttributeFeatures(IEnumerable<IFeature> features)
        {
            _logEntry.AttributeFeatureCount = features.Count(x => x.Attributes != null) * 9;
        }

        public static void LogSampleStart()
        {
            _logEntry.SetSampleStart();
        }

        public static void LogSampleEnd()
        {
            _logEntry.SetSampleEnd();
            _logEntry.SetTotalEnd();
        }

        public static void LogSampleSize(int size)
        {
            _logEntry.SampleSize = size;
        }

        public static void WriteLogEntry()
        {
            var records = new List<LogEntry>
            {
                _logEntry
            };

            if (!File.Exists(@"\log.csv"))
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";"
                };

                using var writer = new StreamWriter(@".\log.csv");
                using var csv = new CsvWriter(writer, config);
                csv.WriteRecords(records);
            }
            else
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                    Delimiter = ";"
                };
                using var stream = File.Open(@".\log.csv", FileMode.Append);
                using var writer = new StreamWriter(stream);
                using var csv = new CsvWriter(writer, config);
                csv.WriteRecords(records);
            }
        }

        private class LogEntry
        {
            private DateTime _partitionStart;
            private DateTime _partitionEnd;
            private DateTime _selectionStart;
            private DateTime _selectionEnd;
            private DateTime _missingFeatureStart;
            private DateTime _missingFeatureEnd;
            private DateTime _sampleStart;
            private DateTime _sampleEnd;
            private DateTime _totalStart;
            private DateTime _totalEnd;

            public int PartitionCount { get; private set; }
            public int[] CombinedPartitions { get; private set; }
            public int MaxSelectedFeatures { get; private set; }
            public int Interactions { get; private set; }

            public double PartitionTime => (_partitionEnd - _partitionStart).TotalMilliseconds;
            public double SelectionTime => (_selectionEnd - _selectionStart).TotalMilliseconds;
            public double MissingFeatureTime => (_missingFeatureEnd - _missingFeatureStart).TotalMilliseconds;
            public double SampleTime => (_sampleEnd - _sampleStart).TotalMilliseconds;
            public double TotalTime => (_totalEnd - _totalStart).TotalMilliseconds;
            public int InitialFeatureCount { get; set; }
            public int SelectedFeatureCount { get; set; }
            public int MissingFeatureCount { get; set; }
            public int AttributeFeatureCount { get; set; }
            public int ReducedFeatureCount => SelectedFeatureCount + MissingFeatureCount + AttributeFeatureCount;
            public int SampleSize { get; set; }

            public void SetPartitionStart() => _partitionStart = DateTime.Now;
            public void SetPartitionEnd() => _partitionEnd = DateTime.Now;
            public void SetSelectionStart() => _selectionStart = DateTime.Now;
            public void SetSelectionEnd() => _selectionEnd = DateTime.Now;
            public void SetMissingFeatureStart() => _missingFeatureStart = DateTime.Now;
            public void SetMissingFeatureEnd() => _missingFeatureEnd = DateTime.Now;
            public void SetSampleStart() => _sampleStart = DateTime.Now;
            public void SetSampleEnd() => _sampleEnd = DateTime.Now;
            public void SetTotalStart() => _totalStart = DateTime.Now;
            public void SetTotalEnd() => _totalEnd = DateTime.Now;

            public void LogParameters(CommandLineOptions.CommandLineOptions options)
            {
                PartitionCount = options.PartitionCount;
                CombinedPartitions = options.CombinedPartition.ToArray();
                MaxSelectedFeatures = options.MaxSelectedFeatures;
                Interactions = options.Interactions;
            }
        }
    }
}