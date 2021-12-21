using System.Collections.Generic;
using System.Xml.Linq;
using Masterarbeit.Classes.HospitalData.Xml;
using Masterarbeit.Interfaces.HospitalData;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.HospitalData
{
    public class HospitalDatabaseFromXml : IHospitalDatabase
    {
        private readonly string _path;
        private IHospitalDatabase _hospitalDatabase;

        public HospitalDatabaseFromXml(string path)
        {
            _path = path;
        }

        public IEnumerable<IService> Services => DeserializedBaseData().Services;

        private IHospitalDatabase DeserializedBaseData()
        {
            if (_hospitalDatabase != null)
                return _hospitalDatabase;

            var reader = new System.Xml.Serialization.XmlSerializer(typeof(HospitalDataXml));
            var file = XDocument.Load(new System.IO.StreamReader(_path));

            _hospitalDatabase = new HospitalDatabaseFromDeserializedHospitalDatabase((HospitalDataXml)reader.Deserialize(file.CreateReader()));
            return _hospitalDatabase;
        }
    }
}