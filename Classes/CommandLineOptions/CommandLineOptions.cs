using System.Collections.Generic;
using CommandLine;

namespace Masterarbeit.Classes.CommandLineOptions
{
    public class CommandLineOptions
    {
        [Option(shortName: 's', longName: "Statistic", Required = false, HelpText = "Statistic File Path", Default = "./Statistic.xml")]
        public string StatisticPath { get; set; }
        
        [Option(shortName: 'h', longName: "HospitalDatabase", Required = false, HelpText = "HospitalDatabase File Path", Default = "./HospitalData.xml")]
        public string HospitalDatabasePath { get; set; }

        [Option(shortName: 'a', longName: "Attributes", Required = false, HelpText = "Attribute File Path", Default = "./Attributes.xml")]
        public string AttributesPath { get; set; }

        [Option(shortName: 'c', longName: "PartitionCount", Required = false, HelpText = "Number of Partitions Created", Default = 100)]
        public int PartitionCount { get; set; }

        [Option(shortName: 'p', longName: "CombinedPartitions", Required = false, HelpText = "Ids of Partitions to be Combined", Default = new[] {0,1})]
        public IEnumerable<int> CombinedPartition { get; set; }

        [Option(shortName: 'm', longName: "MaxSelectedFeatures", Required = false, HelpText = "Maximum Number of Features selected for the Feature Model", Default = 500)]
        public int MaxSelectedFeatures { get; set; }
        
        [Option(shortName: 'g', longName: "GenerateSample", Required = false, HelpText = "Option to generate a sample from reduced Feature-Model", Default = false)]
        public bool GenerateSample { get; set; }

        [Option(shortName: 't', longName: "t-wise-interactions", Required = false, HelpText = "Strength of the generated sample", Default = 2)]
        public int Interactions { get; set; }
    }
}