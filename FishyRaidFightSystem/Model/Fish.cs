using FishyRaidFightSystem.Model.Spells;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FishyRaidFightSystem.Model
{
    public class Fish
    {

        //Játékban lévő tulajdonságok

        public int Elet { get; set; }
        public int Ero { get; set; }
        public int Level { get; set; }
        public int EXP { get; set; }
        public int Maxhp { get; set; }
        public string Levelformat { get; set; }
        public bool delegalthozzaadva { get; set; }

        public Lovedek lovedeke { get; set; }
        public string kepszam { get; set; }
        public Spell Tavolsagi { get; set; }
        public Spell Buff { get; set; }
        public string Eleresiut { get; set; }
        public bool elfoglalt { get; set; }
        public int sorszam { get; set; }
        public int pozicio { get; set; }
        bool novelbvagycsokkent;
        public bool meghalt { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double oldx { get; set; }
        public double oldy { get; set; }
        //  public Fish Hala { get; set; }
        public bool tamad { get; set; }
        public bool visszamegy { get; set; }
        public int Kozelsebzes { get; set; }
        public int Helye { get; set; }
        public object locker;
        public string regieleres { get; set; }
        public bool csikmutat { get; set; }
        public string dead { get; set; }
        public string tavolsagitipus { get; set; }
        public string bufftipus { get; set; }



        public DispatcherTimer dt = new DispatcherTimer(); //Támadásért felel
        public DispatcherTimer vegzo = new DispatcherTimer();

        public Spell Kozelharci { get; set; }


        //--------------------Képesség effektek--------------------
        public string Kepessegeffekt { get; set; }
        //---------------------------------------------------------
        AudioFileReader punch;
        WaveOut punchout;
        string fishbonepath;
        string bubblepath;
        public Fish()
        {
            this.Kozelharci = new Trackle(this);
            this.Levelformat = EXP + "/" + Level * 100;
            this.kepszam = "1";
            this.Tavolsagi = new DoubleBubble(this);
            this.delegalthozzaadva = false;
            this.Buff = new GainEnergy(this);
            this.Maxhp = 100;
            this.elfoglalt = false;
            this.Ero = 10;
            this.Kepessegeffekt = "no";
            string punchpath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Music", "punch.wav");
            this.punch = new AudioFileReader(punchpath);
            punchout = new WaveOut();
            punchout.Init(punch);
            this.csikmutat = true;
            this.novelbvagycsokkent = true;
            this.meghalt = false;
            this.x = 0;
            this.y = 0;
            this.tamad = false;
            this.visszamegy = false;
            this.locker = new object();
            fishbonepath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "fishbone.png");
            this.dead = fishbonepath;
            bubblepath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "bubble.png");
            this.lovedeke = new Lovedek(this.x, this.y, bubblepath);


            dt.Interval = TimeSpan.FromMilliseconds(1);
            vegzo.Interval = TimeSpan.FromMilliseconds(100);

        }
        public void poziciokezel()
        {
            if (pozicio < 30 && novelbvagycsokkent == true)
            {
                pozicio++;

            }
            if (pozicio > 0 && novelbvagycsokkent == false)
            {
                pozicio--;
            }
            if (pozicio == 30)
            {
                novelbvagycsokkent = false;
                pozicio--;
            }
            if (pozicio == 0)
            {
                novelbvagycsokkent = true;
                pozicio++;
            }

        }
        public void Tamad(Fish mit)
        {
            this.oldx = x;
            this.oldy = y;
            this.tamad = true;
            this.csikmutat = false;
            int mitx = (int)mit.x;
            int mity = (int)mit.y;
            bool sebzett = false;
            this.elfoglalt = true;


            if (!delegalthozzaadva)
            {
                dt.Tick += delegate
                {
                    int szamlalo = 0;

                    while (szamlalo < 10)
                    {
                        if (visszamegy == false)
                        {
                            if (x != mitx)
                            {
                                if (x > mitx)
                                {
                                    x--;
                                }
                                else if (x < mitx) { x++; }
                            }

                            if (y != mity)
                            {
                                if (y > mity)
                                {
                                    y--;
                                }
                                else if (y < mity) { y++; }
                            }
                        }
                        if (visszamegy == true)
                        {
                            if (!sebzett)
                            {
                                mit.Elet -= Kozelsebzes;
                                punch.CurrentTime = new TimeSpan(0L);
                                punchout.Play();

                                if (mit.Elet <= 0)
                                {
                                    mit.Elet = 0;
                                    mit.meghalt = true;
                                    mit.Eleresiut = fishbonepath;
                                }
                                sebzett = true;

                            }

                            if ((int)x == (int)oldx && (int)y == (int)oldy)
                            {
                                this.tamad = false;
                                this.csikmutat = true;
                                this.visszamegy = false;
                                dt.Stop();
                            }

                            if (x != oldx)
                            {
                                if (x > oldx)
                                {
                                    x--;
                                }
                                else if (x < oldx) { x++; }
                            }

                            if (y != oldy)
                            {
                                if (y > oldy)
                                {
                                    y--;
                                }
                                else if (y < oldy) { y++; }
                            }
                        }
                        szamlalo++;
                    }
                };
            }



            if (!delegalthozzaadva)
            {
                vegzo.Tick += delegate
                {
                    if (!this.tamad)
                    {
                        this.elfoglalt = false;
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


        public void Loves(Fish mit)
        {


            this.oldx = x;
            this.oldy = y;
            this.tamad = true;
            this.csikmutat = false;
            int mitx = (int)mit.x;
            int mity = (int)mit.y;
            bool sebzett = false;
            Task t = new Task(() =>
            {
                while (this.tamad != false)
                {

                    if (visszamegy == false)
                    {
                        if (x != mitx)
                        {
                            if (x > mitx)
                            {
                                x--;
                            }
                            else if (x < mitx) { x++; }
                        }

                        if (y != mity)
                        {
                            if (y > mity)
                            {
                                y--;
                            }
                            else if (y < mity) { y++; }
                        }
                    }
                    if (visszamegy == true)
                    {
                        if (!sebzett)
                        {
                            mit.Elet -= Kozelsebzes;
                            punch.CurrentTime = new TimeSpan(0L);
                            punchout.Play();

                            if (mit.Elet <= 0)
                            {
                                mit.Elet = 0;
                                mit.meghalt = true;
                                mit.Eleresiut = fishbonepath;
                            }
                            sebzett = true;

                        }

                        if ((int)x == (int)oldx && (int)y == (int)oldy)
                        {
                            this.tamad = false;
                            this.csikmutat = true;
                            this.visszamegy = false;
                        }

                        if (x != oldx)
                        {
                            if (x > oldx)
                            {
                                x--;
                            }
                            else if (x < oldx) { x++; }
                        }

                        if (y != oldy)
                        {
                            if (y > oldy)
                            {
                                y--;
                            }
                            else if (y < oldy) { y++; }
                        }
                    }
                    Thread.Sleep(1);
                }
            });
            t.Start();
        }

        public void Eleresiutatcserel(string mire)
        {
            this.Eleresiut = mire;
        }

        public void Szintetlep()
        {
            this.Ero += 10;
            this.Elet += 10;
            this.Level += 1;
            this.EXP = 0;
        }




    }
}
