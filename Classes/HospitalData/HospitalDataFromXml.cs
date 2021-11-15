using System.Collections.Generic;
using System.Xml.Linq;
using Masterarbeit.Classes.HospitalData.Xml;
using Masterarbeit.Interfaces.BaseData;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.HospitalData
{
    public class HospitalDataFromXml : IHospitalData
    {
        private readonly string _path;
        private IHospitalData _hospitalData;

        public HospitalDataFromXml(string path)
        {
            _path = path;
        }

        public IEnumerable<IService> Services => DeserializedBaseData().Services;

        private IHospitalData DeserializedBaseData()
        {
            if (_hospitalData != null)
                return _hospitalData;

            var reader = new System.Xml.Serialization.XmlSerializer(typeof(HospitalDataXml));
            var file = XDocument.Load(new System.IO.StreamReader(_path));

            _hospitalData = new HospitalDataFromDeserializedHospitalData((HospitalDataXml)reader.Deserialize(file.CreateReader()));
            return _hospitalData;
        }
    }
}