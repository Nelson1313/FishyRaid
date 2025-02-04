using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using FishyRaidFightSystem.Model;
using System.Threading;
using FishyRaidFightSystem.Model.Spells;
using System.IO;
using System.Collections.ObjectModel;

namespace FishyRaidFightSystem.Logic
{
    internal class GameLogic : IGameModel
    {
        public event EventHandler Changed;
        public string kapottexp { get; set; }
        public Player Jatekos { get; set; }
        public ObservableCollection<Fish> PlayerFish { get; set; }
        public ObservableCollection<Fish> EnemyFish { get; set; }
        public int sorszam { get; set; }
        static Random R = new Random();
        public int Palyaszam { get; set; }
        public int MaxPalyaszam { get; set; }
        public int Korszam { get; set; }
        public string KovetkezoHal { get; set; }
        public string melyikpalya { get; set; } //Ez alapján dől el, hogy milyen enemy jöjjön
        public Enemy Enemy { get; set; }

        public bool Jatekvege { get; set; }
        public bool Nyert { get; set; }
        public string Gamemode { get; set; }

        public enum Controls
        {
            First, Second, Third, Fourth
        }

        public GameLogic(string mod, string levelszam)
        {
            this.kapottexp = "10";
            this.melyikpalya = levelszam;

            // this.Jatekos = new Player();
            //  PlayerSave();

           // this.Gamemode = "arena";
            //  this.melyikpalya = "2";
            this.Jatekvege = false;
            this.Nyert = false;
            this.Gamemode = mod;
            this.Jatekos = PlayerLoad();

            // this.Jatekos = new Player();
            //  PlayerSave();


            this.Enemy = new Enemy();
            this.Korszam = 0;
            Task korszamkarbantarto = new Task(() =>
              {

                  while (!Jatekvege)
                  {
                      if (Korszam == 1 && Enemy.FishesInFight[0].meghalt)
                      {
                          Korszam++;
                      }
                      if (Korszam == 3 && Enemy.FishesInFight[1].meghalt)
                      {
                          Korszam++;
                      }
                      if (Korszam == 5 && Enemy.FishesInFight[2].meghalt)
                      {
                          Korszam = 0;
                      }
                      if (Korszam == 0 && Jatekos.FishesInFight[0].meghalt)
                      {
                          Korszam++;
                      }
                      if (Korszam == 2 && Jatekos.FishesInFight[1].meghalt)
                      {
                          Korszam++;
                      }
                      if (Korszam == 4 && Jatekos.FishesInFight[2].meghalt)
                      {
                          Korszam = 0;
                      }
                      Thread.Sleep(300);
                  }
              });
            korszamkarbantarto.Start();
            this.Palyaszam = 1;
            this.MaxPalyaszam = 1;
            //  this.PlayerFish = new List<Fish>();
            this.EnemyFish = new ObservableCollection<Fish>();
            //  PlayerFish.Add(new Fish() { Elet = 100, sorszam = 1, Eleresiut = "fishmodel.png", regieleres = "fishmodel.png", pozicio=0, Kozelsebzes=10,Helye=1});
            //  PlayerFish.Add(new Fish() { Elet = 100, sorszam = 2, Eleresiut = "fishmodel.png", regieleres = "fishmodel.png", pozicio=30, Kozelsebzes=10,Helye=2});
            //  PlayerFish.Add(new Fish() { Elet = 100, sorszam = 3, Eleresiut = "fishmodel.png",regieleres = "fishmodel.png", pozicio=20, Kozelsebzes=10,Helye=3});
            //  EnemyFish.Add(new Fish() { Elet = 100, sorszam = 1, Eleresiut = "polip.png", regieleres = "polip.png", pozicio=10, Kozelsebzes=10,Helye=1});
            //  EnemyFish.Add(new Fish() { Elet = 100, sorszam = 2, Eleresiut = "polip.png", regieleres = "polip.png", pozicio=23, Kozelsebzes=10,Helye=2});
            //  EnemyFish.Add(new Fish() { Elet = 100, sorszam = 3, Eleresiut = "polip.png",regieleres = "polip.png", pozicio=30, Kozelsebzes=10,Helye=3});

            this.sorszam = 0;
            if (Jatekos.FishesInFight[0].Tavolsagi != null)
            {
                KovetkezoHal = "1. TRACKLE 2. " + Jatekos.FishesInFight[0].Tavolsagi.Nev;
            }
            else { KovetkezoHal = "1. TRACKLE"; }
            if (Gamemode != "arena")
            {
                LevelLoad();
            }
            else
            {
                ArenaGenerator();
            }
        }

        public Player PlayerLoad()
        {
            string filePath = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName, "player.json");
            Player p = (Player)SaveAndReadPlayer.Read(typeof(Player), filePath);
            Player p2 = new Player();
            foreach (var item in p.FishesInFight)
            {
                //Néhány dolog mentéskor sérül a régi objektumokban, így újakat hozok létre, amelyek törlik a hibás tulajdonságokat


                if (item.Tavolsagi != null)
                {
                    item.Tavolsagi.Hala = item;
                }
                if (item.Buff != null)
                {
                    item.Buff.Hala = item;
                }
                if (item.Kozelharci != null)
                {
                    item.Kozelharci.Hala = item;
                }
                Fish fishy = new Fish() { Elet = item.Maxhp, sorszam = item.sorszam, Eleresiut = item.Eleresiut, regieleres = item.regieleres, pozicio = item.pozicio, Kozelsebzes = item.Kozelsebzes, Helye = item.Helye, Tavolsagi = item.Tavolsagi, Buff = item.Buff, Level = item.Level, EXP = item.EXP, Ero = item.Ero, Maxhp = item.Maxhp, kepszam=item.kepszam };
                if (fishy.Tavolsagi != null)
                {
                    fishy.Tavolsagi.Hala = fishy;
                }
                if (fishy.Buff != null)
                {
                    fishy.Buff.Hala = fishy;
                }
                if (fishy.Kozelharci is Trackle)
                {
                    fishy.Kozelharci = new Trackle(fishy);
                }
                p2.FishesInFight.Add(fishy);
            }
            p2.AllFishes = p.AllFishes;
            p2.SeaCoin = p.SeaCoin;
            return p2;
        }

        public void ArenaGenerator()
        {
            int szamlalo = 0;
            Enemy.FishesInFight = new ObservableCollection<Fish>();
            Random R = new Random();
            int maxhp = 0;
            int maxero = 0;
            int maxlvl = 0;
            int maxkozelsebzes = 0;
            foreach (var item in Jatekos.FishesInFight)
            {
                if (item.Ero > maxero)
                {
                    maxero = item.Ero;
                }
                if (item.Maxhp > maxhp)
                {
                    maxhp = item.Maxhp;
                }
                if (item.Level > maxlvl)
                {
                    maxlvl = item.Level;
                }
                if (item.Kozelsebzes > maxkozelsebzes)
                {
                    maxkozelsebzes = item.Kozelsebzes;
                }
            }
           
               for(int i = 0; i < 3; i++)
            {
                Fish enemyfish = new Fish();
                enemyfish.Ero = R.Next(1, maxero + 10);
                enemyfish.Kozelsebzes = R.Next(1, maxkozelsebzes + 10);
                int hp = R.Next(20, maxhp + 30);
                enemyfish.Maxhp = hp;
                enemyfish.Elet = hp;

                if (maxlvl < 5)
                {
                    int melyik = R.Next(1, 51);
                    enemyfish.Eleresiut = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", melyik + ".png");
                    enemyfish.regieleres = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", melyik + ".png");
                }

                if (i == 0)
                {
                    enemyfish.sorszam = 1;
                    enemyfish.pozicio = 10;
                    enemyfish.Helye = 1;

                }
                else if (i == 1)
                {
                    enemyfish.sorszam = 2;
                    enemyfish.pozicio = 23;
                    enemyfish.Helye = 2;
                }
                else
                {
                    enemyfish.sorszam = 3;
                    enemyfish.pozicio = 30;
                    enemyfish.Helye = 3;
                }
                Enemy.FishesInFight.Add(enemyfish);
            }
            
        }

        public void GameoverCheck()
        {
            int halottszam = 0;
            foreach (var item in Enemy.FishesInFight)
            {
                if (item.Eleresiut == item.dead)
                {
                    halottszam++;
                }
            }
            if (halottszam == 3)
            {
             
                Nyert = true;
                GameEnd();
            }
           
                halottszam = 0;
                foreach (var item in Jatekos.FishesInFight)
                {
                    if (item.Eleresiut == item.dead)
                    {
                        halottszam++;
                    }
                }
                if (halottszam == 3)
                {
                    Nyert = false;
                    GameEnd();
                }
            


        }

        public void GameEnd()
        {
            if (Nyert)
            {
                Random R = new Random();
                if (Gamemode == "arena")
                {
                    int max = 1;
                    foreach (var item in Jatekos.FishesInFight)
                    {
                        if (item.Level > max)
                        {
                            max = item.Level;
                        }
                    }
                    int mennyi = R.Next(10, max * 10);
                    this.kapottexp = mennyi.ToString();
                    AddExp(Jatekos.FishesInFight, mennyi);
                }
                else { 

                    int szam = R.Next(1, 101);
                    if (szam <= 3000)
                    {
                        int melyik = 0;
                        
                        if (melyikpalya == "1")
                        {
                            melyik = R.Next(1, 51);

                            string kepe = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Fishes", melyik + ".png");
                            Fish reward = new Fish()
                            {
                                Elet = R.Next(40, 110),
                                kepszam=melyik.ToString(),
                                Eleresiut = kepe,
                                regieleres=kepe,
                                Ero = R.Next(5, 13),
                                Kozelsebzes = R.Next(5, 13),
                                Level = 1,
                                Buff = null,
                                Tavolsagi = null,
                              
                            };
                            Jatekos.AllFishes.Add(reward);
                            this.kapottexp = "10";
                            AddExp(Jatekos.FishesInFight, 10);
                        }
                        else if (melyikpalya == "2")
                        {
                            melyik = R.Next(1, 51);
                            string kepe = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Fishes", melyik + ".png");
                            Fish reward = new Fish()
                            {
                                Elet = R.Next(60, 150),
                                Eleresiut = kepe,
                                regieleres=kepe,
                                Ero = R.Next(9, 26),
                                Kozelsebzes = R.Next(9, 26),
                                Level = 1,
                                Buff = null,
                                Tavolsagi = null,
                                kepszam=melyik.ToString()
                            };
                            int legyentavolsagi = R.Next(1, 3);
                            if (legyentavolsagi != 1)
                            {
                                reward.Tavolsagi = new DoubleBubble(reward);
                            }
                            int legyenbuff = R.Next(1, 3);
                            if (legyenbuff != 1)
                            {
                                int milyenbuff = R.Next(0, 5);
                                if (milyenbuff == 0)
                                {
                                    reward.Buff = new GainEnergy(reward);
                                }
                                else
                                {
                                    reward.Buff = new LittleHealth(reward);
                                }
                            }

                            Jatekos.AllFishes.Add(reward);
                            this.kapottexp = "20";
                            AddExp(Jatekos.FishesInFight, 20);

                        }
                        else if (melyikpalya == "3")
                        {
                            
                           

                            melyik = R.Next(1, 51);
                            string kepe = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Fishes", melyik + ".png");
                            Fish reward = new Fish()
                            {
                                Elet = R.Next(110, 190),
                                Eleresiut = kepe,
                                regieleres = kepe,
                                Ero = R.Next(20, 50),
                                Kozelsebzes = R.Next(20, 50),
                                Level = 1,
                                Buff = null,
                                Tavolsagi = null,
                                kepszam = melyik.ToString()
                            };
                            int legyentavolsagi = R.Next(1, 3);
                            if (legyentavolsagi != 1)
                            {
                                int milyentavolsagi = R.Next(0, 2);
                                if (milyentavolsagi == 0)
                                {
                                    reward.Tavolsagi = new DoubleBubble(reward);
                                }
                                else
                                {
                                    reward.Tavolsagi = new RandomBubble(reward);
                                }
                                
                            }
                            int legyenbuff = R.Next(1, 3);
                            if (legyenbuff != 1)
                            {
                                int milyenbuff = R.Next(0, 5);
                                if (milyenbuff == 0)
                                {
                                    reward.Buff = new GainEnergy(reward);
                                }
                                else if(milyenbuff==1||milyenbuff==2||milyenbuff==3)
                                {
                                    reward.Buff = new LittleHealth(reward);
                                }
                                else
                                {
                                    reward.Buff = new Health(reward);
                                }
                            }
                            this.kapottexp = "30";
                            AddExp(Jatekos.FishesInFight, 30);
                            Jatekos.AllFishes.Add(reward);
                        }
                        else if (melyikpalya == "4")
                        {
                            melyik = R.Next(50, 101);
                            string kepe = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Fishes", melyik + ".png");
                            Fish reward = new Fish()
                            {
                                Elet = R.Next(230, 280),
                                Eleresiut = kepe,
                                regieleres = kepe,
                                Ero = R.Next(30, 70),
                                Kozelsebzes = R.Next(30, 70),
                                Level = 1,
                                Buff = null,
                                Tavolsagi = null,
                                kepszam = melyik.ToString()
                            };
                            int legyentavolsagi = R.Next(1, 3);
                            if (legyentavolsagi != 1)
                            {
                                int milyentavolsagi = R.Next(0, 3);
                                if (milyentavolsagi == 0)
                                {
                                    reward.Tavolsagi = new DoubleBubble(reward);
                                }
                                else if (milyentavolsagi == 0)
                                {
                                    reward.Tavolsagi = new TrippleBubble(reward);
                                }
                                else
                                {
                                    reward.Tavolsagi = new RandomBubble(reward);
                                }

                            }
                            int legyenbuff = R.Next(1, 3);
                            if (legyenbuff != 1)
                            {
                                int milyenbuff = R.Next(0, 5);
                                if (milyenbuff == 0)
                                {
                                    reward.Buff = new GainEnergy(reward);
                                }
                                else if (milyenbuff == 1 || milyenbuff == 2 || milyenbuff == 3)
                                {
                                    reward.Buff = new LittleHealth(reward);
                                }
                                else
                                {
                                    reward.Buff = new Health(reward);
                                }
                            }
                            this.kapottexp = "40";
                            AddExp(Jatekos.FishesInFight, 40);
                            Jatekos.AllFishes.Add(reward);
                        }
                        else if (melyikpalya == "5")
                        {
                            melyik = R.Next(50, 101);
                            string kepe = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Fishes", melyik + ".png");
                            Fish reward = new Fish()
                            {
                                Elet = R.Next(320, 320),
                                Eleresiut = kepe,
                                regieleres = kepe,
                                Ero = R.Next(45, 78),
                                Kozelsebzes = R.Next(45, 78),
                                Level = 1,
                                Buff = null,
                                Tavolsagi = null,
                                kepszam = melyik.ToString()
                            };
                            int legyentavolsagi = R.Next(1, 3);
                            if (legyentavolsagi != 1)
                            {
                                int milyentavolsagi = R.Next(0, 4);
                                if (milyentavolsagi == 0)
                                {
                                    reward.Tavolsagi = new DoubleBubble(reward);
                                }
                                else if (milyentavolsagi == 1)
                                {
                                    reward.Tavolsagi = new TrippleBubble(reward);
                                }
                                else if (milyentavolsagi == 2)
                                {
                                    reward.Tavolsagi = new BubbleKiss(reward);
                                }
                                else
                                {
                                    reward.Tavolsagi = new RandomBubble(reward);
                                }

                            }
                            int legyenbuff = R.Next(1, 3);
                            if (legyenbuff != 1)
                            {
                                int milyenbuff = R.Next(0, 5);
                                if (milyenbuff == 0)
                                {
                                    reward.Buff = new GainEnergy(reward);
                                }
                                else if (milyenbuff == 1 )
                                {
                                    reward.Buff = new LittleHealth(reward);
                                }
                                else if(milyenbuff == 2 || milyenbuff == 3)
                                {
                                    reward.Buff = new Health(reward);
                                }
                                else
                                {
                                    reward.Buff = new MaxHealth(reward);
                                }
                            }
                            this.kapottexp = "50";
                            AddExp(Jatekos.FishesInFight, 50);
                            Jatekos.AllFishes.Add(reward);
                        }
                        else if (melyikpalya == "6")
                        {
                            melyik = R.Next(151, 191);
                            string kepe = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Fishes", melyik + ".png");
                            Fish reward = new Fish()
                            {
                                Elet = R.Next(450, 470),
                                Eleresiut = kepe,
                                regieleres = kepe,
                                Ero = R.Next(55, 86),
                                Kozelsebzes = R.Next(55, 86),
                                Level = 1,
                                Buff = null,
                                Tavolsagi = null,
                                kepszam = melyik.ToString()
                            };
                            int legyentavolsagi = R.Next(1, 3);
                            if (legyentavolsagi != 1)
                            {
                                int milyentavolsagi = R.Next(0, 5);
                                if (milyentavolsagi == 0)
                                {
                                    reward.Tavolsagi = new DoubleBubble(reward);
                                }
                                else if (milyentavolsagi == 1)
                                {
                                    reward.Tavolsagi = new TrippleBubble(reward);
                                }
                                else if (milyentavolsagi == 2)
                                {
                                    reward.Tavolsagi = new BubbleKiss(reward);
                                }
                                else if (milyentavolsagi == 3)
                                {
                                    reward.Tavolsagi = new BabyThrow(reward);
                                }
                                else
                                {
                                    reward.Tavolsagi = new RandomBubble(reward);
                                }

                            }
                            int legyenbuff = R.Next(1, 3);
                            if (legyenbuff != 1)
                            {
                                int milyenbuff = R.Next(0, 5);
                                if (milyenbuff == 0)
                                {
                                    reward.Buff = new GainEnergy(reward);
                                }
                                else if (milyenbuff == 1)
                                {
                                    reward.Buff = new LittleHealth(reward);
                                }
                                else if (milyenbuff == 2 || milyenbuff == 3)
                                {
                                    reward.Buff = new Health(reward);
                                }
                                else
                                {
                                    reward.Buff = new MaxHealth(reward);
                                }
                            }
                            this.kapottexp = "60";
                            AddExp(Jatekos.FishesInFight, 60);
                            Jatekos.AllFishes.Add(reward);
                        }
                        else if (melyikpalya == "7")
                        {
                            melyik = R.Next(151, 191);
                            string kepe = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Fishes", melyik + ".png");
                            Fish reward = new Fish()
                            {
                                Elet = R.Next(490, 540),
                                Eleresiut = kepe,
                                regieleres = kepe,
                                Ero = R.Next(65, 98),
                                Kozelsebzes = R.Next(65, 98),
                                Level = 1,
                                Buff = null,
                                Tavolsagi = null,
                                kepszam = melyik.ToString()
                            };
                            int legyentavolsagi = R.Next(1, 3);
                            if (legyentavolsagi != 1)
                            {
                                int milyentavolsagi = R.Next(0, 5);
                                if (milyentavolsagi == 0)
                                {
                                    reward.Tavolsagi = new DoubleBubble(reward);
                                }
                                else if (milyentavolsagi == 1)
                                {
                                    reward.Tavolsagi = new TrippleBubble(reward);
                                }
                                else if (milyentavolsagi == 2)
                                {
                                    reward.Tavolsagi = new BubbleKiss(reward);
                                }
                                else if (milyentavolsagi == 3)
                                {
                                    reward.Tavolsagi = new BabyThrow(reward);
                                }
                                else
                                {
                                    reward.Tavolsagi = new RandomBubble(reward);
                                }

                            }
                            int legyenbuff = R.Next(1, 3);
                            if (legyenbuff != 1)
                            {
                                int milyenbuff = R.Next(0, 5);
                                if (milyenbuff == 0)
                                {
                                    reward.Buff = new GainEnergy(reward);
                                }
                                else if (milyenbuff == 1)
                                {
                                    reward.Buff = new LittleHealth(reward);
                                }
                                else if (milyenbuff == 2 || milyenbuff == 3)
                                {
                                    reward.Buff = new Health(reward);
                                }
                                else
                                {
                                    reward.Buff = new MaxHealth(reward);
                                }
                            }
                            this.kapottexp = "70";
                            AddExp(Jatekos.FishesInFight, 50);
                            Jatekos.AllFishes.Add(reward);
                        }
                    }
                }
                
                      PlayerSave();
                      Jatekvege = true;
                 
            }
            else
            {
                PlayerSave();
                Jatekvege = true;
            }
            /*  Task vegveto = new Task(() =>
              {


                  bool mehet = false;
                  while (!mehet)
                  {
                      bool jo = true;
                      foreach (var item in Enemy.FishesInFight)
                      {
                          if (item.tamad != false)
                          {
                              jo = false;
                          }
                      }
                      foreach (var item in Jatekos.FishesInFight)
                      {
                          if (item.tamad != false)
                          {
                              jo = false;
                          }
                      }

                      if (jo)
                      {
                          mehet = true;
                      }
                  }
                  PlayerSave();
                  Jatekvege = true;
              });
              vegveto.Start();*/



        }

        public void AddExp(IList<Fish> halak, int mennyi)
        {
            foreach (var item in halak)
            {
                if ((item.EXP + mennyi) >= item.Level * 100)
                {
                    item.Level += 1;
                    item.Ero += 15;
                    item.Elet += 30;
                    item.Kozelsebzes += 15;
                    item.EXP = (item.EXP + mennyi) - 100;
                }
                else
                {
                    item.EXP += mennyi;
                }
            }


            /*     foreach (var item in p.AllFishes)
                 {
                     item.Tavolsagi.Hala = item;
                     item.Buff.Hala = item;
                 }
                 return p;*/

        }

        public void PlayerSave()
        {
            string filePath = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName, "player.json");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            foreach (var item in this.Jatekos.FishesInFight)
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
                    if(item.Kozelharci is Trackle)
                    {
                        item.Kozelharci = new Trackle(item);
                        item.Kozelharci.Hala = null;
                    }
                }
                item.Elet = item.Maxhp;
                item.Eleresiut = item.regieleres;
            }
            foreach (var item in this.Jatekos.AllFishes)
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
                item.Eleresiut = item.regieleres;
            }

            SaveAndReadPlayer.Save(Jatekos, filePath);
        }

        public void LevelLoad()
        {
            if (melyikpalya == "1")
            {
                string imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "jellyfish.png");
                Enemy.FishesInFight = new ObservableCollection<Fish>();
                Enemy.FishesInFight.Add(new Fish() { Elet = 60, sorszam = 1, Eleresiut = imgPath, regieleres = imgPath, pozicio = 10, Kozelsebzes = 10, Helye = 1, Level=1, Maxhp=60 });
                Enemy.FishesInFight.Add(new Fish() { Elet = 60, sorszam = 2, Eleresiut = imgPath, regieleres = imgPath, pozicio = 23, Kozelsebzes = 10, Helye = 2, Level = 1, Maxhp=60 });
                Enemy.FishesInFight.Add(new Fish() { Elet = 60, sorszam = 3, Eleresiut = imgPath, regieleres = imgPath, pozicio = 30, Kozelsebzes = 10, Helye = 3, Level = 1, Maxhp=60 });
                int szamlalo = 1;
                foreach (var item in Enemy.FishesInFight)
                {
                    
                        item.Tavolsagi = new RandomBubble(item);
                    

                    szamlalo++;

                }
            }
            else if (melyikpalya == "2")
            {
                string imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "polip.png");
                Enemy.FishesInFight = new ObservableCollection<Fish>();
                Enemy.FishesInFight.Add(new Fish() { Elet = 160, sorszam = 1, Eleresiut = imgPath, regieleres = imgPath, pozicio = 10, Ero=30, Kozelsebzes = 20, Helye = 1 , Level=4, Maxhp=160});
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "jellyfish.png");
                Enemy.FishesInFight.Add(new Fish() { Elet = 120, sorszam = 2, Eleresiut = imgPath, regieleres = imgPath, pozicio = 23, Ero = 20, Kozelsebzes = 20, Helye = 2, Level = 4, Maxhp=120 });
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "jellyfish.png");
                Enemy.FishesInFight.Add(new Fish() { Elet = 120, sorszam = 3, Eleresiut = imgPath, regieleres = imgPath, pozicio = 30, Ero = 20, Kozelsebzes = 20, Helye = 3, Level = 4, Maxhp=120 });
                foreach (var item in Enemy.FishesInFight)
                {
                    item.Tavolsagi = new DoubleBubble(item);
                }
            }
            else if (melyikpalya == "3")
            {
                string imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "polip.png");
                Enemy.FishesInFight = new ObservableCollection<Fish>();
                Enemy.FishesInFight.Add(new Fish() { Elet = 160, sorszam = 1, Eleresiut = imgPath, regieleres = imgPath, Ero = 40, pozicio = 10, Kozelsebzes = 20, Helye = 1, Level = 1, Maxhp = 160 });
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "jellyfish.png");
                Enemy.FishesInFight.Add(new Fish() { Elet = 140, sorszam = 2, Eleresiut = imgPath, regieleres = imgPath, Ero = 30, pozicio = 23, Kozelsebzes = 20, Helye = 2, Level = 1, Maxhp = 140 });
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "polip.png");
                Enemy.FishesInFight.Add(new Fish() { Elet = 160, sorszam = 3, Eleresiut = imgPath, regieleres = imgPath, Ero = 40, pozicio = 30, Kozelsebzes = 20, Helye = 3, Level = 1, Maxhp = 160 });
                foreach (var item in Enemy.FishesInFight)
                {
                    item.Tavolsagi = new DoubleBubble(item);
                }
                Enemy.FishesInFight[1].Tavolsagi = new RandomBubble(Enemy.FishesInFight[1]);
            }
            else if (melyikpalya == "4")
            {
                string imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "sponge.png");
                Enemy.FishesInFight = new ObservableCollection<Fish>();
                Enemy.FishesInFight.Add(new Fish() { Elet = 240, sorszam = 1, Eleresiut = imgPath, regieleres = imgPath, Ero = 50, pozicio = 10, Kozelsebzes = 60, Helye = 1, Level = 1, Maxhp = 240 });
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "polip.png");
                Enemy.FishesInFight.Add(new Fish() { Elet = 160, sorszam = 2, Eleresiut = imgPath, regieleres = imgPath, Ero = 40, pozicio = 23, Kozelsebzes = 40, Helye = 2, Level = 1, Maxhp = 160 });
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "polip.png");
                Enemy.FishesInFight.Add(new Fish() { Elet = 160, sorszam = 3, Eleresiut = imgPath, regieleres = imgPath, Ero = 40, pozicio = 30, Kozelsebzes = 40, Helye = 3, Level = 1, Maxhp = 160 });
                foreach (var item in Enemy.FishesInFight)
                {
                    item.Tavolsagi = new RandomBubble(item);
                }
                Enemy.FishesInFight[0].Tavolsagi = new BubbleKiss(Enemy.FishesInFight[0]);
            }
            else if (melyikpalya == "5")
            {
                string imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "polip.png");
                Enemy.FishesInFight = new ObservableCollection<Fish>();
                Enemy.FishesInFight.Add(new Fish() { Elet = 360, sorszam = 1, Eleresiut = imgPath, regieleres = imgPath, Ero = 60, pozicio = 10, Kozelsebzes = 60, Helye = 1, Level = 1, Maxhp = 360 });
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "sponge.png");
                Enemy.FishesInFight.Add(new Fish() { Elet = 160, sorszam = 2, Eleresiut = imgPath, regieleres = imgPath, Ero = 40, pozicio = 23, Kozelsebzes = 40, Helye = 2, Level = 1, Maxhp = 160 });
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "sponge.png");
                Enemy.FishesInFight.Add(new Fish() { Elet = 360, sorszam = 3, Eleresiut = imgPath, regieleres = imgPath, Ero = 60, pozicio = 30, Kozelsebzes = 40, Helye = 3, Level = 1, Maxhp = 360 });
                foreach (var item in Enemy.FishesInFight)
                {
                    item.Tavolsagi = new DoubleBubble(item);
                }
                Enemy.FishesInFight[0].Tavolsagi = new RandomBubble(Enemy.FishesInFight[0]);
                Enemy.FishesInFight[2].Tavolsagi = new RandomBubble(Enemy.FishesInFight[2]);
            }
            else if (melyikpalya == "6")
            {
                string imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "axolotl.png");
                Enemy.FishesInFight = new ObservableCollection<Fish>();
                Enemy.FishesInFight.Add(new Fish() { Elet = 540, sorszam = 1, Eleresiut = imgPath, regieleres = imgPath, Ero = 50, pozicio = 10, Kozelsebzes = 60, Helye = 1, Level = 1, Maxhp = 540 });
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "sponge.png");
                Enemy.FishesInFight.Add(new Fish() { Elet = 360, sorszam = 2, Eleresiut = imgPath, regieleres = imgPath, Ero = 80, pozicio = 23, Kozelsebzes = 80, Helye = 2, Level = 1, Maxhp = 360 });
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "sponge.png");
                Enemy.FishesInFight.Add(new Fish() { Elet = 360, sorszam = 3, Eleresiut = imgPath, regieleres = imgPath, Ero = 80, pozicio = 30, Kozelsebzes = 80, Helye = 3, Level = 1, Maxhp = 360 });

                Enemy.FishesInFight[0].Tavolsagi = new BabyThrow(Enemy.FishesInFight[0]);
                Enemy.FishesInFight[1].Tavolsagi = new RandomBubble(Enemy.FishesInFight[0]);
                Enemy.FishesInFight[2].Tavolsagi = new RandomBubble(Enemy.FishesInFight[2]);
            }
            else if (melyikpalya == "7")
            {
                string imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "axolotl.png");
                Enemy.FishesInFight = new ObservableCollection<Fish>();
                Enemy.FishesInFight.Add(new Fish() { Elet = 860, sorszam = 1, Eleresiut = imgPath, regieleres = imgPath, Ero = 50, pozicio = 10, Kozelsebzes = 60, Helye = 1, Level = 1, Maxhp = 540 });
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "sponge.png");
                Enemy.FishesInFight.Add(new Fish() { Elet = 720, sorszam = 2, Eleresiut = imgPath, regieleres = imgPath, Ero = 80, pozicio = 23, Kozelsebzes = 80, Helye = 2, Level = 1, Maxhp = 360 });
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "axolotl.png");
                Enemy.FishesInFight.Add(new Fish() { Elet = 860, sorszam = 3, Eleresiut = imgPath, regieleres = imgPath, Ero = 80, pozicio = 30, Kozelsebzes = 80, Helye = 3, Level = 1, Maxhp = 360 });

                Enemy.FishesInFight[0].Tavolsagi = new BabyThrow(Enemy.FishesInFight[0]);
                Enemy.FishesInFight[1].Tavolsagi = new RandomBubble(Enemy.FishesInFight[0]);
                Enemy.FishesInFight[2].Tavolsagi = new BabyThrow(Enemy.FishesInFight[2]);
            }

        }

        System.Windows.Size Area; //Méretezi a hátteret

        public void SetupSizes(System.Windows.Size area)
        {
            this.Area = area;

        }

        public void TimeStep()
        {

            foreach (var hal in Jatekos.FishesInFight)
            {
                Rect fishrect = new Rect(hal.x, hal.y, 100, 100);
                Rect fishlovedekrect = new Rect(hal.lovedeke.x, hal.lovedeke.y, 100, 100);

                foreach (var enemy in Enemy.FishesInFight)
                {
                    Rect enemyrect = new Rect(enemy.x, enemy.y, 100, 100);
                    Rect enemylovedekrect = new Rect(enemy.lovedeke.x, enemy.lovedeke.y, 100, 100);
                    if (fishrect.IntersectsWith(enemyrect))
                    {
                        bool teljesult = false;
                        if (hal.tamad)
                        {
                            hal.visszamegy = true;
                            teljesult = true;
                            string regi = enemy.Eleresiut;
                            Task t = new Task(() =>
                            {
                                string hitpath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "punch.png");
                                // enemy.Eleresiut = "punch.png";
                                enemy.Eleresiut = hitpath;
                                Thread.Sleep(50);

                            });

                            t.Start();

                            Task csere = new Task(() =>
                            {
                                t.Wait();
                                Thread.Sleep(80);
                                //     enemy.Eleresiut = regi;
                                if (!enemy.meghalt)
                                {
                                    enemy.Eleresiut = enemy.regieleres;
                                }
                                else
                                {
                                    enemy.Eleresiut = enemy.dead;
                                }


                            });
                            csere.Start();

                        }
                        else if (enemy.tamad && !teljesult)
                        {
                            enemy.visszamegy = true;
                            string regi = hal.Eleresiut;
                            Task t = new Task(() =>
                            {


                                hal.Eleresiut = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "punch.png");
                                Thread.Sleep(50);
                                // hal.Eleresiutatcserel(regi);



                            }, TaskCreationOptions.LongRunning);
                            t.Start();

                            Task csere = new Task(() =>
                            {
                                t.Wait();
                                Thread.Sleep(80);
                                // hal.Eleresiut = regi;
                                if (!hal.meghalt)
                                {
                                    hal.Eleresiut = hal.regieleres;
                                }
                                else
                                {
                                    hal.Eleresiut = hal.dead;
                                }

                            }, TaskCreationOptions.LongRunning);
                            csere.Start();
                        }

                    }

                    /*    if (fishlovedekrect.IntersectsWith(enemyrect)&&enemy.tamad==true)   //Itt lehet, hogy gond lesz
                        {
                            hal.tamad = false;
                            hal.lovedeke.aktiv = false;
                        }
                        else if (enemylovedekrect.IntersectsWith(fishrect) && hal.tamad == true)
                        {
                            enemy.tamad = false;
                            enemy.lovedeke.aktiv = false;
                        }*/

                    if (fishlovedekrect.IntersectsWith(enemyrect) && hal.lovedeke.x != hal.x && hal.lovedeke.y != hal.x)   //Itt lehet, hogy gond lesz
                    {
                        // hal.tamad = false;
                        hal.lovedeke.aktiv = false;
                    }
                    else if (enemylovedekrect.IntersectsWith(fishrect) && enemy.lovedeke.x != enemy.x && enemy.lovedeke.y != enemy.x)
                    {
                        // enemy.tamad = false;
                        enemy.lovedeke.aktiv = false;
                    }

                }
            }

            if ((Korszam == 1 || Korszam == 3 || Korszam == 5) && Jatekvege == false) //Ellenséges halak támadási logikája
            {
                int melyik = 0; //Itt kell beállítani a textet
                if (Jatekos.FishesInFight[0].Tavolsagi != null)
                {
                    KovetkezoHal = "1. TRACKLE 2. " + Jatekos.FishesInFight[0].Tavolsagi.Nev;
                }
                else { KovetkezoHal = "1. TRACKLE"; }
                if (Korszam == 3)
                {
                    melyik = 1;
                    if (Jatekos.FishesInFight[1].Tavolsagi != null)
                    {
                        KovetkezoHal = "1. TRACKLE 2. " + Jatekos.FishesInFight[1].Tavolsagi.Nev;
                    }
                    else { KovetkezoHal = "1. TRACKLE"; }
                }
                else if (Korszam == 5)
                {
                    melyik = 2;
                    if (Jatekos.FishesInFight[2].Tavolsagi != null)
                    {
                        KovetkezoHal = "1. TRACKLE 2. " + Jatekos.FishesInFight[2].Tavolsagi.Nev;
                    }
                    else { KovetkezoHal = "1. TRACKLE"; }
                }

                if (Enemy.FishesInFight[melyik].Eleresiut == Enemy.FishesInFight[melyik].dead)
                {
                    if (melyik + 1 < Enemy.FishesInFight.Count)
                    {
                        if (!(Enemy.FishesInFight[melyik + 1].Eleresiut == Enemy.FishesInFight[melyik + 1].dead))
                        {
                            melyik += 1;
                        }
                    }
                    else if (melyik - 1 >= 0)
                    {
                        if (!(Enemy.FishesInFight[melyik - 1].Eleresiut == Enemy.FishesInFight[melyik - 1].dead))
                        {
                            melyik -= 1;
                        }
                    }
                    else
                    {
                        //Pálya vége
                    }
                }

                if (Enemy.FishesInFight[melyik].Elet <= 0)
                {
                    Enemy.FishesInFight[melyik].meghalt = true;
                    Enemy.FishesInFight[melyik].Eleresiut = Fishbone.AddFishbonepath();
                }

                bool tamade = false;
                foreach (var item in Enemy.FishesInFight)
                {
                    if (item.tamad)
                    {
                        tamade = true;
                    }
                }
                if (!tamade)
                {
                    int melyiket = 0;
                    bool vantarsa = false;
                    foreach (var item in Jatekos.FishesInFight)
                    {
                        if (vantarsa == false && item.Helye == Enemy.FishesInFight[melyik].Helye && item.meghalt == false)
                        {
                            vantarsa = true;
                            melyiket = item.Helye - 1;
                        }
                        else if (item.meghalt == false && vantarsa == false)
                        {
                            melyiket = item.Helye - 1;
                        }

                    }

                    Task tamad = new Task(() => //Ez a baj
                    {
                        if (Enemy.FishesInFight[melyik].meghalt == false && Enemy.FishesInFight[melyik].Eleresiut != System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "fishbone.png"))
                        {
                            Enemy.FishesInFight[melyik].tamad = true;
                            // Thread.Sleep(5500);
                            bool mehet = false;

                            while (!mehet)
                            {
                                int szabadok = 0;
                                foreach (var item in Jatekos.FishesInFight)
                                {
                                    if (!item.elfoglalt)
                                    {
                                        szabadok++;
                                    }
                                }
                                if (szabadok == 3)
                                {
                                    //   Thread.Sleep(2000);
                                    mehet = true;
                                }
                                else { Thread.Sleep(700); }
                            }

                            if (Jatekos.FishesInFight[melyiket].Eleresiut!= Jatekos.FishesInFight[melyiket].dead)
                            {
                                if ((Enemy.FishesInFight[melyik].Eleresiut == Enemy.FishesInFight[melyik].dead))
                                {
                                  //  Enemy.FishesInFight[melyik].tamad = false; - Lehet, hogy vissza kell állítani

                                    if (melyik + 1 < Enemy.FishesInFight.Count)
                                    {
                                        if (!(Enemy.FishesInFight[melyik + 1].Eleresiut == Enemy.FishesInFight[melyik + 1].dead))
                                        {
                                            melyik += 1;
                                        }
                                    }
                                    else if (melyik - 1 >= 0)
                                    {
                                        if (!(Enemy.FishesInFight[melyik - 1].Eleresiut == Enemy.FishesInFight[melyik - 1].dead))
                                        {
                                            melyik -= 1;
                                        }
                                    }
                                }

                                if (!Jatekvege)
                                {
                                    Enemy.FishesInFight[melyik].Kozelharci.Tamad(Jatekos.FishesInFight[melyiket],Jatekos.FishesInFight);
                                }
                            }
                            else
                            {
                                if (melyiket + 1 < Jatekos.FishesInFight.Count)
                                {
                                    if (Jatekos.FishesInFight[melyiket + 1].Eleresiut!= Jatekos.FishesInFight[melyiket+1].dead)
                                    {

                                        if ((Enemy.FishesInFight[melyik].Eleresiut == Enemy.FishesInFight[melyik].dead))
                                        {
                                            Enemy.FishesInFight[melyik].tamad = false;

                                            if (melyik + 1 < Enemy.FishesInFight.Count)
                                            {
                                                if (!(Enemy.FishesInFight[melyik + 1].Eleresiut == Enemy.FishesInFight[melyik + 1].dead))
                                                {
                                                    melyik += 1;
                                                }
                                            }
                                            else if (melyik - 1 >= 0)
                                            {
                                                if (!(Enemy.FishesInFight[melyik - 1].Eleresiut == Enemy.FishesInFight[melyik - 1].dead))
                                                {
                                                    melyik -= 1;
                                                }
                                            }
                                        }

                                        if (!Jatekvege)
                                        {
                                            Enemy.FishesInFight[melyik].Kozelharci.Tamad(Jatekos.FishesInFight[melyiket + 1],Jatekos.FishesInFight);
                                        }
                                    }

                                }
                                else if (melyiket - 1 >= 0)
                                {
                                    if (Jatekos.FishesInFight[melyiket - 1].Eleresiut!= Jatekos.FishesInFight[melyiket-1].dead)
                                    {
                                        if ((Enemy.FishesInFight[melyik].Eleresiut == Enemy.FishesInFight[melyik].dead))
                                        {
                                            Enemy.FishesInFight[melyik].tamad = false;

                                            if (melyik + 1 < Enemy.FishesInFight.Count)
                                            {
                                                if (!(Enemy.FishesInFight[melyik + 1].Eleresiut == Enemy.FishesInFight[melyik + 1].dead))
                                                {
                                                    melyik += 1;
                                                }
                                            }
                                            else if (melyik - 1 >= 0)
                                            {
                                                if (!(Enemy.FishesInFight[melyik - 1].Eleresiut == Enemy.FishesInFight[melyik - 1].dead))
                                                {
                                                    melyik -= 1;
                                                }
                                            }
                                        }

                                        if (!Jatekvege)
                                        {
                                            Enemy.FishesInFight[melyik].Kozelharci.Tamad(Jatekos.FishesInFight[melyiket - 1],Jatekos.FishesInFight);
                                        }
                                    }
                                }
                            }

                        }

                    });

                    Task tavolsagi = new Task(() =>
                    {
                        if (Enemy.FishesInFight[melyik].meghalt == false && Enemy.FishesInFight[melyik].Eleresiut != System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "fishbone.png"))
                        {
                            Enemy.FishesInFight[melyik].tamad = true;
                            // Thread.Sleep(5500);
                            bool mehet = false;

                            while (!mehet)
                            {
                                int szabadok = 0;
                                foreach (var item in Jatekos.FishesInFight)
                                {
                                    if (!item.elfoglalt)
                                    {
                                        szabadok++;
                                    }
                                }
                                if (szabadok == 3)
                                {
                                    Thread.Sleep(2000);
                                    mehet = true;
                                }
                                else { Thread.Sleep(700); }
                            }

                            if (Jatekos.FishesInFight[melyiket].meghalt == false)
                            {
                                if ((Enemy.FishesInFight[melyik].Eleresiut == Enemy.FishesInFight[melyik].dead))
                                {
                                    Enemy.FishesInFight[melyik].tamad = false;

                                    if (melyik + 1 < Enemy.FishesInFight.Count)
                                    {
                                        if (!(Enemy.FishesInFight[melyik + 1].Eleresiut == Enemy.FishesInFight[melyik + 1].dead))
                                        {
                                            melyik += 1;
                                        }
                                    }
                                    else if (melyik - 1 >= 0)
                                    {
                                        if (!(Enemy.FishesInFight[melyik - 1].Eleresiut == Enemy.FishesInFight[melyik - 1].dead))
                                        {
                                            melyik -= 1;
                                        }
                                    }
                                }
                                if (!Jatekvege)
                                {
                                    Enemy.FishesInFight[melyik].Tavolsagi.Tamad(Jatekos.FishesInFight[melyiket], Jatekos.FishesInFight);
                                }


                            }
                            else
                            {
                                if (melyiket + 1 < Jatekos.FishesInFight.Count)
                                {
                                    if (Jatekos.FishesInFight[melyiket + 1].meghalt == false)
                                    {

                                        if ((Enemy.FishesInFight[melyik].Eleresiut == Enemy.FishesInFight[melyik].dead))
                                        {
                                            Enemy.FishesInFight[melyik].tamad = false;

                                            if (melyik + 1 < Enemy.FishesInFight.Count)
                                            {
                                                if (!(Enemy.FishesInFight[melyik + 1].Eleresiut == Enemy.FishesInFight[melyik + 1].dead))
                                                {
                                                    melyik += 1;
                                                }
                                            }
                                            else if (melyik - 1 >= 0)
                                            {
                                                if (!(Enemy.FishesInFight[melyik - 1].Eleresiut == Enemy.FishesInFight[melyik - 1].dead))
                                                {
                                                    melyik -= 1;
                                                }
                                            }
                                        }

                                        Enemy.FishesInFight[melyik].Tavolsagi.Tamad(Jatekos.FishesInFight[melyiket + 1], Jatekos.FishesInFight);
                                    }

                                }
                                else if (melyiket - 1 >= 0)
                                {
                                    if (Jatekos.FishesInFight[melyiket - 1].meghalt == false)
                                    {

                                        if ((Enemy.FishesInFight[melyik].Eleresiut == Enemy.FishesInFight[melyik].dead))
                                        {
                                            Enemy.FishesInFight[melyik].tamad = false;

                                            if (melyik + 1 < Enemy.FishesInFight.Count)
                                            {
                                                if (!(Enemy.FishesInFight[melyik + 1].Eleresiut == Enemy.FishesInFight[melyik + 1].dead))
                                                {
                                                    melyik += 1;
                                                }
                                            }
                                            else if (melyik - 1 >= 0)
                                            {
                                                if (!(Enemy.FishesInFight[melyik - 1].Eleresiut == Enemy.FishesInFight[melyik - 1].dead))
                                                {
                                                    melyik -= 1;
                                                }
                                            }
                                        }

                                        if (!Jatekvege)
                                        {
                                            Enemy.FishesInFight[melyik].Tavolsagi.Tamad(Jatekos.FishesInFight[melyiket - 1], Jatekos.FishesInFight);
                                        }
                                    }
                                }
                            }

                        }

                    });

                    if (Enemy.FishesInFight[melyik].meghalt == false)
                    {
                        bool mehet = false;

                        while (!mehet)
                        {
                            int szam = R.Next(0, 2);
                            if (szam == 0)
                            {
                                tamad.Start();
                                mehet = true;
                            }
                            else if (szam == 1 && Enemy.Energy >= Enemy.FishesInFight[melyik].Tavolsagi.Energiakoltseg)
                            {
                                tavolsagi.Start();
                                Task varo = new Task(() =>
                                {
                                    tavolsagi.Wait();
                                    Enemy.FishesInFight[melyik].tamad = false;
                                    Enemy.Energy -= Enemy.FishesInFight[melyik].Tavolsagi.Energiakoltseg;
                                });
                                varo.Start();
                                mehet = true;
                            }
                        }
                    }
                    
                    if (Korszam == 5)
                    {
                        if(!Jatekos.FishesInFight[0].meghalt)
                        {
                            Korszam = 0;
                        }
                        else if (!Jatekos.FishesInFight[1].meghalt)
                        {
                            Korszam = 2;
                        }
                        else if(!Jatekos.FishesInFight[2].meghalt)
                        {
                            Korszam = 4;
                        }

                        
                    }
                    else if (Korszam == 1)
                    {
                        if (!Jatekos.FishesInFight[1].meghalt)
                        {
                            Korszam = 2;
                        }
                        else if (!Jatekos.FishesInFight[0].meghalt)
                        {
                            Korszam = 0;
                        }
                        else if (!Jatekos.FishesInFight[2].meghalt)
                        {
                            Korszam = 4;
                        }
                    }
                    else if (Korszam == 3)
                    {
                        if (!Jatekos.FishesInFight[2].meghalt)
                        {
                            Korszam = 4;
                        }
                        else if (!Jatekos.FishesInFight[0].meghalt)
                        {
                            Korszam = 0;
                        }
                        else if (!Jatekos.FishesInFight[1].meghalt)
                        {
                            Korszam = 2;
                        }

                    }
                    else {

                        if (Korszam == 5)
                        {
                            Korszam = 0;
                        }
                        else
                        {
                            Korszam++;
                        }
                    
                    }




                    Task Novelo = new Task(() =>
                    {
                        while (Enemy.FishesInFight[melyik].tamad == true)
                        {
                            Thread.Sleep(300);
                        }
                        Jatekos.Energy += 1;
                    });
                    Novelo.Start();



                }
            }

            Changed?.Invoke(this, null);
        }

        public void FightEngine()
        {

        }

        public void Control(Controls control)
        {
            switch (control)
            {
                case Controls.First:


                    bool vanetamado = false;
                    foreach (var item in Jatekos.FishesInFight)
                    {
                        if (item.tamad)
                        {
                            vanetamado = true;
                        }
                    }
                    foreach (var item in Enemy.FishesInFight)
                    {
                        if (item.tamad)
                        {
                            vanetamado = true;
                        }
                    }
                    if (Korszam == 0 && !vanetamado || Korszam == 2 && !vanetamado || Korszam == 4 && !vanetamado)
                    {
                        int hely = 0;
                        if (Korszam == 2)
                        {
                            hely = 1;
                        }
                        else if (Korszam == 4)
                        {
                            hely = 2;
                        }
                        int melyiket = 0;
                        bool vantarsa = false;
                        foreach (var item in Enemy.FishesInFight)
                        {
                            if (vantarsa == false && item.Helye == Jatekos.FishesInFight[hely].Helye && item.meghalt == false)
                            {
                                vantarsa = true;
                                melyiket = item.Helye - 1;
                            }
                            else if (item.meghalt == false && vantarsa == false)
                            {
                                melyiket = item.Helye - 1;
                            }


                        }

                        if (Enemy.FishesInFight[melyiket].Eleresiut != Fishbone.AddFishbonepath())
                        {
                            Jatekos.FishesInFight[hely].Kozelharci.Tamad(Enemy.FishesInFight[melyiket], Enemy.FishesInFight);
                        }
                        else
                        {
                            int index = 0;
                            for(int i = 0; i < Enemy.FishesInFight.Count; i++)
                            {
                                if (Enemy.FishesInFight[i].Eleresiut != Fishbone.AddFishbonepath())
                                {
                                    index = i;
                                }
                            }
                            Jatekos.FishesInFight[hely].Kozelharci.Tamad(Enemy.FishesInFight[index], Enemy.FishesInFight);
                        }

                        Enemy.Energy++;
                         Korszam++;

                        

                    }

                    break;
                case Controls.Second:

                    vanetamado = false;
                    foreach (var item in Jatekos.FishesInFight)
                    {
                        if (item.tamad)
                        {
                            vanetamado = true;
                        }
                    }
                    foreach (var item in Enemy.FishesInFight)
                    {
                        if (item.tamad)
                        {
                            vanetamado = true;
                        }
                    }
                    if (Korszam == 0 && !vanetamado || Korszam == 2 && !vanetamado || Korszam == 4 && !vanetamado)
                    {


                        int hely = 0;
                        if (Korszam == 2)
                        {
                            hely = 1;
                        }
                        else if (Korszam == 4)
                        {
                            hely = 2;
                        }
                        int melyiket = 0;
                        bool vantarsa = false;
                        foreach (var item in Enemy.FishesInFight)
                        {
                            if (vantarsa == false && item.Helye == Jatekos.FishesInFight[hely].Helye && item.meghalt == false)
                            {
                                vantarsa = true;
                                melyiket = item.Helye - 1;
                            }
                            else if (item.meghalt == false && vantarsa == false)
                            {
                                melyiket = item.Helye - 1;
                            }


                        }
                        if (Jatekos.FishesInFight[hely].Tavolsagi != null)
                        {
                            if (Jatekos.FishesInFight[hely].meghalt == false && Jatekos.Energy >= Jatekos.FishesInFight[hely].Tavolsagi.Energiakoltseg)
                            {
                                Jatekos.FishesInFight[hely].lovedeke.aktiv = true;
                                Jatekos.FishesInFight[hely].Tavolsagi.Tamad(Enemy.FishesInFight[melyiket], Enemy.FishesInFight); //Itt még állítani kell
                                Jatekos.Energy -= Jatekos.FishesInFight[hely].Tavolsagi.Energiakoltseg;
                                Enemy.Energy++;
                                Korszam++;
                                if (Jatekos.FishesInFight[hely].Buff==null)
                                {
                                    MessageBox.Show("This Fish doesn't have Buff ability!");
                                }
                                if (Jatekos.FishesInFight[hely].Buff!=null)
                                {
                                    Jatekos.FishesInFight[hely].Buff.Befejezett = false;
                                }
                                
                            }
                            else if (Jatekos.Energy < Jatekos.FishesInFight[hely].Tavolsagi.Energiakoltseg)
                            {
                                MessageBox.Show("You don't have enough Energy to use that Buff!");
                            }
                        }
                        





                    }


                    break;
                case Controls.Third:

                    vanetamado = false;
                    foreach (var item in Jatekos.FishesInFight)
                    {
                        if (item.tamad)
                        {
                            vanetamado = true;
                        }
                    }
                    foreach (var item in Enemy.FishesInFight)
                    {
                        if (item.tamad)
                        {
                            vanetamado = true;
                        }
                    }

                    if (Korszam == 0 && !vanetamado || Korszam == 2 && !vanetamado || Korszam == 4 && !vanetamado)
                    {
                        int hely = 0;
                        if (Korszam == 2)
                        {
                            hely = 1;
                        }
                        else if (Korszam == 4)
                        {
                            hely = 2;
                        }

                        if (Jatekos.FishesInFight[hely].Buff != null)
                        {
                            if (!Jatekos.FishesInFight[hely].Buff.Befejezett)
                            {
                                Jatekos.FishesInFight[hely].Buff.Buff(Jatekos.FishesInFight[hely], Jatekos);
                                Jatekos.Energy -= Jatekos.FishesInFight[hely].Buff.Energiakoltseg;
                                Jatekos.FishesInFight[hely].Buff.Befejezett = true;
                                if (Jatekos.FishesInFight[hely].Buff.KorszamotNovelo)
                                {
                                    Korszam++;
                                }
                                Jatekos.FishesInFight[hely].Buff.mutat = true;
                            }
                        }
                    }

                    break;
                case Controls.Fourth:

                    break;
                default:
                    break;
            }
            Changed?.Invoke(this, null); //El kell sütni a frissítés miatt az eseményt

        }
    }
}
