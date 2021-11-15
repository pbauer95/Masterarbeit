﻿using System.Collections.Generic;
using System.Linq;
using Masterarbeit.Classes.HospitalData.Xml;
using Masterarbeit.Classes.Service;
using Masterarbeit.Interfaces.BaseData;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Classes.HospitalData
{
    public class HospitalDataFromDeserializedHospitalData : IHospitalData
    {
        private readonly HospitalDataXml _hospitalDataXml;
        private IEnumerable<IService> _services;

        public HospitalDataFromDeserializedHospitalData(HospitalDataXml hospitalDataXml)
        {
            _hospitalDataXml = hospitalDataXml;
        }

        public IEnumerable<IService> Services => ConvertedServices();

        private IEnumerable<IService> ConvertedServices()
        {
            if (_services != null)
                return _services;

            return _services = _hospitalDataXml.HospitalDataServices.Select(x => new ServiceFromDeserializedBaseData(x));
        }
    }
}