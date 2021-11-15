﻿using System.Xml.Serialization;

namespace Masterarbeit.Classes.MasterData.Xml
{
    public class MasterDataFabXml
    {
        [XmlElement("Name")] public string Name { get; set; }
        [XmlElement("Share")] public decimal Share { get; set; }
    }
}