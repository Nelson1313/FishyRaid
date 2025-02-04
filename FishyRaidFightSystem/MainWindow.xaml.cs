using FishyRaidFightSystem.Logic;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static FishyRaidFightSystem.Logic.GameLogic;
using AfterFightMenu;

namespace FishyRaidFightSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameLogic logic;
        string Palyaszam;
        System.Media.SoundPlayer player;
        System.Media.SoundPlayer startMenuMusicPlayer;
        public bool IsClosed { get; set; }
        public MainWindow(int palyaszam) // pálya szám a konstruktorba
        {
            InitializeComponent();
            this.Palyaszam = palyaszam.ToString();
            logic = new GameLogic("seadungeon", Palyaszam.ToString());
            logic.Palyaszam = Convert.ToInt32(palyaszam);
            this.Palyaszam = palyaszam.ToString();
            string musicpath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Music", "music.wav");
            player = new System.Media.SoundPlayer(musicpath);
            player.PlayLooping();
        }
         
        public MainWindow() //Arena
        {
            InitializeComponent();          
            logic = new GameLogic("arena","no");
            logic.melyikpalya = "0";
            logic.Palyaszam = 0;
            string musicpath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Music", "music.wav");
            player = new System.Media.SoundPlayer(musicpath);
            player.PlayLooping();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
          //  logic = new GameLogic();
          //  logic.melyikpalya = Palyaszam;
            if (logic.Palyaszam != 0)
            {
                logic.Gamemode = "seadungeon";
            }
            logic.LevelLoad();
            Display.SetupModel(logic);
            DispatcherTimer dt = new DispatcherTimer(); //Időzítő
            dt.Interval = TimeSpan.FromMilliseconds(5);
            dt.Tick += Dt_Tick; //Így kell időzítőt kezelni
            dt.Start();
            Display.SetupSizes(new Size((int)Grid.ActualWidth, (int)Grid.ActualHeight));
            logic.SetupSizes(new Size((int)Grid.ActualWidth, (int)Grid.ActualHeight));

            DispatcherTimer vegveto = new DispatcherTimer();
            vegveto.Interval = TimeSpan.FromMilliseconds(1); //Megvárja, amíg befejeződnek a képességek
            vegveto.Tick += delegate
             {
                 if (logic.Jatekvege)
                 {
                     if (logic.Nyert)
                     {
                         Window win = new AfterFightMenu.MainWindow(logic.kapottexp);
                         this.Close();                         
                         vegveto.Stop();
                         win.Show();
                     }
                     else
                     {
                         Window lost = new AfterFightMenu.YouLostWindow();
                         this.Close();                         
                         vegveto.Stop();
                         lost.Show();
                     }
                 }
                 else
                 {
                     logic.GameoverCheck();
                 }
             };
            vegveto.Start();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (logic != null)
            {
                Display.SetupSizes(new Size((int)Grid.ActualWidth, (int)Grid.ActualHeight));
                logic.SetupSizes(new Size((int)Grid.ActualWidth, (int)Grid.ActualHeight));
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D1)
            {
                logic.Control(Controls.First);
            }
            else if (e.Key == Key.D2)
            {
                logic.Control(Controls.Second);
            }
            else if (e.Key == Key.D3)
            {
                logic.Control(Controls.Third);
            }
        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            logic.TimeStep();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            player.Stop();
            string menumusic = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Music", "MapMusic.wav");
            startMenuMusicPlayer = new System.Media.SoundPlayer(menumusic);
            startMenuMusicPlayer.PlayLooping();            
        }
    }
}
