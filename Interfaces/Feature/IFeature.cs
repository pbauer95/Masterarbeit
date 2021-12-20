using System.Collections.Generic;
using System.Xml.Linq;
using Masterarbeit.Interfaces.Attribute;
using Masterarbeit.Interfaces.Fab;
using Masterarbeit.Interfaces.Service;

namespace Masterarbeit.Interfaces.Feature
{
    public interface IFeature
    {
        string Name => GeneratedName();
        IService Service { get; }
        IFab Fab { get; }
        bool AbstractionLevel { get; }
        bool Global { get; }
        bool Freeze { get; }
        bool Mandatory { get; }
        IEnumerable<IAttribute> Attributes { get; }
        decimal CaseMix { get; }

        private string GeneratedName()
        {
            var name = Service.Type + Service.Code;

            if (Global)
            {
                name += "Global";
            }
            else
            {
                name += AbstractionLevel
                    ? "Abstraktionsebene"
                    //TODO: Hardcoded Standort
                    : Fab != null
                        ? "S0"
                        : "";

                if (Fab != null)
                    name += Fab.Name;
            }

            if (Freeze)
                name += "Freeze";

            return name;
        }
    }
}