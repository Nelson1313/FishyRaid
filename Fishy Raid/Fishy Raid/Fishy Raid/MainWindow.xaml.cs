using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FishyRaidFightSystem;
using DungeonMap;
using TeamEditor;
using FishyRaidFightSystem.Model;
using FishyRaidFightSystem.Model.Spells;

namespace Fishy_Raid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Media.SoundPlayer player;
        public MainWindow()
        {
            ChangePathForThisPc();
            InitializeComponent();
            string musicpath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Music", "MapMusic.wav");
            player = new System.Media.SoundPlayer(musicpath);
            player.PlayLooping();
        }

        private void ChangePathForThisPc() 
        {
           Player p = (Player)SaveAndReadPlayer.Read(typeof(Player), System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName, "player.json"));
            
            foreach (var item in p.AllFishes)
            {               
                item.Eleresiut = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Fishes",item.kepszam+".png");
                item.regieleres= System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Fishes", item.kepszam + ".png");
                item.dead= System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Fishes", "fishbone.png");
            }
            foreach (var item in p.FishesInFight)
            {
                item.Eleresiut = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Fishes", item.kepszam + ".png");
                item.regieleres = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Fishes", item.kepszam + ".png");
                item.dead = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "fishbone.png");
            }

            foreach (var item in p.FishesInFight)
            {
                if (item.Tavolsagi != null)
                {
                    item.Tavolsagi.Hala = null;
                }
                if (item.Buff != null)
                {
                    item.Buff.Hala = null;
                }
                if (item.Kozelharci != null)
                {
                    if (item.Kozelharci is Trackle)
                    {
                        item.Kozelharci = new Trackle(item);
                        item.Kozelharci.Hala = null;
                    }
                }
                item.Elet = item.Maxhp;
            }
            foreach (var item in p.AllFishes)
            {
                if (item.Tavolsagi != null)
                {
                    item.Tavolsagi.Hala = null;
                }
                if (item.Buff != null)
                {
                    item.Buff.Hala = null;
                }
                if (item.Kozelharci != null)
                {
                    if (item.Kozelharci is Trackle)
                    {
                        item.Kozelharci = new Trackle(item);
                        item.Kozelharci.Hala = null;
                    }
                }
            }

            SaveAndReadPlayer.Save(p, System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName, "player.json"));
        }    
        private void Open_New_Arena(object sender, RoutedEventArgs e)
        {
            Window win = new FishyRaidFightSystem.MainWindow();
            win.Show();
            
            
        }



        private void Open_New_SeaDungeon(object sender, RoutedEventArgs e)
        {
            Window dungeon = new DungeonMap.MainWindow();
            dungeon.Show();     
        }

        private void Open_New_Team_Editor(object sender, RoutedEventArgs e) 
        {
            Window editor = new TeamEditor.MainWindow();
            editor.Show();
        }
    }
}
