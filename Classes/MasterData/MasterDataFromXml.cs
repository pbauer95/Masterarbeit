using System.Collections.Generic;
using System.Xml.Linq;
using Masterarbeit.Classes.MasterData.Xml;
using Masterarbeit.Interfaces.MasterData;

namespace Masterarbeit.Classes.MasterData
{
    public class MasterDataFromXml : IMasterData
    {
        private string _path;
        private IMasterData _masterData;

        public MasterDataFromXml(string path)
        {
            _path = path;
        }

        public IEnumerable<IMasterDataService> Services => DeserializedMasterData().Services;

        private IMasterData DeserializedMasterData()
        {
            if (_masterData != null)
                return _masterData;

            var reader = new System.Xml.Serialization.XmlSerializer(typeof(MasterDataXml));
            var file = XDocument.Load(new System.IO.StreamReader(_path));

            _masterData = new MasterDataFromDeserializedMasterData((MasterDataXml)reader.Deserialize(file.CreateReader()));
            return _masterData;
        }
    }
}