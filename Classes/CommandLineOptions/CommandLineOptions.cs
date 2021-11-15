using System.Collections.Generic;
using CommandLine;

namespace Masterarbeit.Classes.CommandLineOptions
{
    public class CommandLineOptions
    {
        [Option(shortName: 'd', longName: "DistributionData", Required = false, HelpText = "Distribution File Path", Default = "./DistributionData.xml")]
        public string DistributionDataPath { get; set; }
        
        [Option(shortName: 'h', longName: "HospitalData", Required = false, HelpText = "HospitalData File Path", Default = "-1")]
        public string HospitalData { get; set; }

        [Option(shortName: 'a', longName: "AttributeData", Required = false, HelpText = "Attribute File Path", Default = "./Attributes.xml")]
        public string AttributeDataPath { get; set; }

        [Option(shortName: 'p', longName: "PartitionCount", Required = false, HelpText = "Number of Partitions Created", Default = 100)]
        public int PartitionCount { get; set; }

        [Option(shortName: 'c', longName: "CombinedPartitions", Required = false, HelpText = "Ids of Partitions to be Combined", Default = new[] {0, 24, 49, 74, 99})]
        public IEnumerable<int> CombinedPartition { get; set; }

        [Option(shortName: 'm', longName: "MaxSelectedFeatures", Required = false, HelpText = "Maximum Number of Features selected for the Feature Model", Default = 999999)]
        public int MaxSelectedFeatures { get; set; }

        [Option(shortName: 'g', longName: "Global", Required = false, HelpText = "True = Create Global Partitions; False = Create Partitions for each Type", Default = false)]
        public bool Global { get; set; }
        
        [Option(shortName: 't', longName: "t-wise-interactions", Required = false, HelpText = "Strength of the generated sample", Default = 2)]
        public int Interactions { get; set; }
    }
}