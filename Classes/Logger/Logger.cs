using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Masterarbeit.Interfaces.Feature;
using Masterarbeit.Interfaces.FeatureModel;

namespace Masterarbeit.Classes.Logger
{
    public static class Logger
    {
        private static LogEntry _logEntry;

        public static void StartLogEntry(IFeatureModel featureModel, CommandLineOptions.CommandLineOptions options)
        {
            var featureList = featureModel.Features.ToList();

            _logEntry = new LogEntry();
            _logEntry.SetTotalStart();
            _logEntry.LogParameters(options);
            _logEntry.InitialFeatureCount = featureList.Count + featureList.Count(x => x.Attributes != null) * 9;
        }

        public static void LogPartitionStart()
        {
            _logEntry.PartitionStart();
        }

        public static void LogPartitionEnd()
        {
            _logEntry.PartitionEnd();
        }

        public static void LogSelectionStart()
        {
            _logEntry.SelectionStart();
        }

        public static void LogSelectionEnd()
        {
            _logEntry.SelectionEnd();
        }

        public static void LogMissingFeaturesStart()
        {
            _logEntry.MissingFeatureStart();
        }

        public static void LogMissingFeaturesEnd()
        {
            _logEntry.MissingFeatureEnd();
        }

        public static void LogReducedFeatureModelStats(IFeatureModel featureModel)
        {
            _logEntry.AttributeFeatureCount = featureModel.Features.Count(x => x.Attributes != null) * 9;
            _logEntry.ReducedFeatureCount = featureModel.Features.Count() + _logEntry.AttributeFeatureCount;
        }

        public static void LogSampleStart()
        {
            _logEntry.SetSampleStart();
        }

        public static void LogSampleEnd()
        {
            _logEntry.SetSampleEnd();
        }

        public static void LogSampleSize(int size)
        {
            _logEntry.SampleSize = size;
        }

        public static void WriteLogEntry()
        {
            _logEntry.SetTotalEnd();

            var records = new List<LogEntry>
            {
                _logEntry
            };

            if (File.Exists(@".\log.csv"))
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";",
                    HasHeaderRecord = false
                };
                using var stream = File.Open(@".\log.csv", FileMode.Append);
                using var writer = new StreamWriter(stream);
                using var csv = new CsvWriter(writer, config);
                csv.WriteRecords(records);
            }
            else
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";"
                };

                using var writer = new StreamWriter(@".\log.csv");
                using var csv = new CsvWriter(writer, config);
                csv.WriteRecords(records);
            }
        }

        private class LogEntry
        {
            private DateTime _partitionStart;
            private DateTime _selectionStart;
            private DateTime _missingFeatureStart;
            private DateTime _sampleStart;
            private DateTime _sampleEnd;
            private DateTime _totalStart;
            private DateTime _totalEnd;

            public int PartitionCount { get; private set; }
            public int CombinedPartitionCount { get; private set; }
            public int MaxSelectedFeatures { get; private set; }
            public int Interactions { get; private set; }

            public int PartitionTime { get; private set; }

            public int SelectionTime { get; private set; }

            public int MissingFeatureTime { get; private set; }

            public int SampleTime => (int)Math.Round((_sampleEnd - _sampleStart).TotalMilliseconds, 0);
            public int TotalTime => (int)Math.Round((_totalEnd - _totalStart).TotalMilliseconds, 0);
            public int InitialFeatureCount { get; set; }
            public int AttributeFeatureCount { get; set; }
            public int ReducedFeatureCount { get; set; }
            public int SampleSize { get; set; }

            public void PartitionStart() => _partitionStart = DateTime.Now;

            public void PartitionEnd()
            {
                var partitionTime = DateTime.Now - _partitionStart;
                PartitionTime += (int)Math.Round(partitionTime.TotalMilliseconds, 0);
            }

            public void SelectionStart() => _selectionStart = DateTime.Now;

            public void SelectionEnd()
            {
                var selectionTime = DateTime.Now - _selectionStart;
                SelectionTime += (int)Math.Round(selectionTime.TotalMilliseconds, 0);
            }

            public void MissingFeatureStart() => _missingFeatureStart = DateTime.Now;

            public void MissingFeatureEnd()
            {
                var missingFeatureTime = DateTime.Now - _missingFeatureStart;
                MissingFeatureTime += (int)Math.Round(missingFeatureTime.TotalMilliseconds, 0);
            }

            public void SetSampleStart() => _sampleStart = DateTime.Now;
            public void SetSampleEnd() => _sampleEnd = DateTime.Now;
            public void SetTotalStart() => _totalStart = DateTime.Now;
            public void SetTotalEnd() => _totalEnd = DateTime.Now;

            public void LogParameters(CommandLineOptions.CommandLineOptions options)
            {
                PartitionCount = options.PartitionCount;
                CombinedPartitionCount = options.CombinedPartition.Count();
                MaxSelectedFeatures = options.MaxSelectedFeatures;
                Interactions = options.Interactions;
            }
        }
    }
}