using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FishyRaidFightSystem.Model.Spells
{
    public class Trackle : Spell
    {

        public DispatcherTimer dt = new DispatcherTimer(); //Támadásért felel
        public DispatcherTimer vegzo = new DispatcherTimer();
        public bool delegalthozzaadva { get; set; }
        public string fishbonepath { get; set; }
        AudioFileReader punch;
        WaveOut punchout;

        public Fish mittamad { get; set; }

        public Trackle(Fish hal)
        {
            this.Hala = hal;
            this.delegalthozzaadva = false;
            string punchpath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Music", "punch.wav");
            this.punch = new AudioFileReader(punchpath);
            punchout = new WaveOut();
            punchout.Init(punch);
            dt.Interval = TimeSpan.FromMilliseconds(10);
            vegzo.Interval = TimeSpan.FromMilliseconds(100);
            fishbonepath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "fishbone.png");
        }

        public override void Buff(Fish mit, Player jatekos)
        {
            throw new NotImplementedException();
        }

        public override void SzovegLeszed()
        {
            throw new NotImplementedException();
        }

        public override void Tamad(Fish mit, ObservableCollection<Fish> halak)
        {
            int index = -1;

            string szoveg2 = Fishbone.AddFishbonepath();

            for (int i = 0; i < halak.Count; i++)
            {
                if (halak[i].Helye == Hala.Helye)
                {
                    if (halak[i].Eleresiut != szoveg2)
                    {
                        index = i;
                    }
                }
            }

            if (index == -1)
            {
                for (int i = 0; i < halak.Count; i++)
                {
                    
                        if (halak[i].Eleresiut != szoveg2)
                        {
                            index = i;
                        }
                    
                }
            }

         //   string szoveg1 = mit.Eleresiut;

           

            if (index!=-1)
            {
                if(halak[index].Eleresiut != szoveg2)
                {
                    string valodieleres = halak[index].Eleresiut;
                    int tamadoindex = 0;

                    for(int i = 0; i < halak.Count; i++)
                    {
                        if (Hala.Helye == halak[i].Helye)
                        {
                            if (halak[i].Eleresiut == valodieleres)
                            {
                                tamadoindex = i;
                                break;
                            }
                        }
                        else if (halak[i].Eleresiut == valodieleres)
                        {
                            tamadoindex = i;
                       
                        }
                    }

                    Csinal(halak[tamadoindex],halak);
                }
            }
            else
            {
                 index = 0;
                Hala.elfoglalt = true;
                for (int i = 0; i < halak.Count; i++)
                {
                    if (halak[i].Eleresiut!= Fishbone.AddFishbonepath())
                    {
                        index = i;
                    }
                }
                
                Csinal(halak[index],halak);
            }

            }

        public void Csinal(Fish mit, ObservableCollection<Fish> halak)
        {



            Hala.oldx = Hala.x;
            Hala.oldy = Hala.y;
            Hala.tamad = true;
            Hala.csikmutat = false;
            int mitx = (int)mit.x;
            int mity = (int)mit.y;
            bool sebzett = false;
            Hala.elfoglalt = true;
            Fish tamadni = mit;
            ;
            this.mittamad = mit;
            if (!delegalthozzaadva)
            {
                dt.Tick += delegate
                {
                    int szamlalo = 0;

                    while (szamlalo < 10)
                    {
                        if (Hala != null)
                        {
                            if (Hala.visszamegy == false)
                            {
                                if (Hala.x != tamadni.x)
                                {
                                    if (Hala.x > tamadni.x)
                                    {
                                        Hala.x--;
                                    }
                                    else if (Hala.x < tamadni.x) { Hala.x++; }
                                }

                                if (Hala.y != tamadni.y)
                                {
                                    if (Hala.y > tamadni.y)
                                    {
                                        Hala.y--;
                                    }
                                    else if (Hala.y < tamadni.y) { Hala.y++; }
                                }
                            }
                            if (Hala.visszamegy == true)
                            {
                                if (!sebzett)
                                {

                                    if (!Hala.meghalt)
                                    {
                                        if (tamadni.meghalt)
                                        {
                                            foreach (var item in halak)
                                            {
                                                if (!item.meghalt)
                                                {
                                                    tamadni = item;
                                                }
                                            }
                                        }
                                        tamadni.Elet -= Hala.Kozelsebzes;
                                        punch.CurrentTime = new TimeSpan(0L);
                                        punchout.Play();
                                    }
                                    

                                    if (tamadni.Elet <= 0)
                                    {
                                        tamadni.Elet = 0;
                                        tamadni.meghalt = true;
                                       // mit.dead = fishbonepath;
                                        tamadni.Eleresiut = Fishbone.AddFishbonepath();
                                    }
                                    sebzett = true;

                                }

                                if ((int)Hala.x == (int)Hala.oldx && (int)Hala.y == (int)Hala.oldy)
                                {
                                    Hala.tamad = false;
                                    Hala.csikmutat = true;
                                    Hala.visszamegy = false;
                                    dt.Stop();
                                }

                                if (Hala.x != Hala.oldx)
                                {
                                    if (Hala.x > Hala.oldx)
                                    {
                                        Hala.x--;
                                    }
                                    else if (Hala.x < Hala.oldx) { Hala.x++; }
                                }

                                if (Hala.y != Hala.oldy)
                                {
                                    if (Hala.y > Hala.oldy)
                                    {
                                        Hala.y--;
                                    }
                                    else if (Hala.y < Hala.oldy) { Hala.y++; }
                                }
                            }
                            szamlalo++;
                        }
                    }
                };
            }



            if (!delegalthozzaadva)
            {
                vegzo.Tick += delegate
                {

                    if (!Hala.tamad)
                    {
                        Hala.elfoglalt = false;
                        sebzett = false;
                        dt.Stop();
                        vegzo.Stop();
                        delegalthozzaadva = true;
                    }

                };
            }
            dt.Start();
            vegzo.Start();

        }
    }
    }

