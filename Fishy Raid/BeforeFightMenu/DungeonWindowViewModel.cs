using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FishyRaidFightSystem.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace BeforeFightMenu
{
    public class DungeonWindowViewModel:ObservableRecipient
    {
        public ObservableCollection<Fish> FishesToBattle { get; set; }

        private Enemy enemy;

        public Enemy Enemies
        {
            get { return enemy; }
            set {
                SetProperty(ref enemy, value);
            }
        }

        public ObservableCollection<Potion> Potions { get; set; }
        public int Stage { get; set; }
        public DungeonWindowViewModel()
        {
           string path = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName, "player.json");
            Player p = (Player)SaveAndReadPlayer.Read(typeof(Player),path);

            foreach (var item in p.AllFishes)
            {
                item.Levelformat = item.EXP + "/" + item.Level * 100;
            }
            foreach (var item in p.FishesInFight)
            {
                item.Levelformat = item.EXP + "/" + item.Level * 100;
            }

            FishesToBattle = new ObservableCollection<Fish>();
            Enemies = new Enemy();
            Potions = new ObservableCollection<Potion>();

            FishesToBattle = p.FishesInFight;
            Potions = p.Potions;

            Enemies = new Enemy();
            Enemies.EnemyLoad(Convert.ToString(Stage));           
        }

        
    }
}
