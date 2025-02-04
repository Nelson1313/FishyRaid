using FishyRaidFightSystem.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishyRaidFightSystem.Logic
{
    internal interface IGameModel
    {
        event EventHandler Changed;

        //        List<Fish> PlayerFish { get; set; }
        ObservableCollection<Fish> EnemyFish { get; set; }
        Player Jatekos { get; set; }
        Enemy Enemy { get; set; }
        int Palyaszam { get; set; }
        int MaxPalyaszam { get; set; }
        string KovetkezoHal { get; set; }
        int Korszam { get; set; }
        string Gamemode { get; set; }
    }
}
