using Masterarbeit.Interfaces.Fab;

namespace Masterarbeit.Classes.Fab
{
    public class Fab : IFab
    {
        public Fab(string name, decimal caseMix)
        {
            Name = name;
            CaseMix = caseMix;
        }

        public string Name { get; }
        public decimal CaseMix { get; }
    }
}