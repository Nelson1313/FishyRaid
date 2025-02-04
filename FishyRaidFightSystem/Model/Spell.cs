using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishyRaidFightSystem.Model
{
    public abstract class Spell
    {
        public Fish Hala { get; set; }

        public bool mutat { get; set; } //mutathatja-e az üzenetet

        public bool mutatasleszedve { get; set; }

        public int minlevel { get; set; } //Minimum szint amitől generálódhat

        public string message { get; set; } //displayre kiirandó üzenet

        public bool KorszamotNovelo { get; set; }

        public bool Befejezett { get; set; }

        public int Energiakoltseg { get; set; }

        public string Nev { get; set; }

        public abstract void Tamad(Fish mit, ObservableCollection<Fish> halak);

        public abstract void SzovegLeszed();

        public abstract void Buff(Fish mit, Player jatekos);
    }
}
