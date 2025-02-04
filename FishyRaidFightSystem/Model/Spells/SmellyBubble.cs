using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace FishyRaidFightSystem.Model.Spells
{
    public class SmellyBubble : Spell
    {
        public override void Buff(Fish mit, Player jatekos)
        {
            throw new NotImplementedException();
        }

        AudioFileReader punch;
        WaveOut punchout;
        AudioFileReader hit;
        WaveOut hitout;

        static Random R = new Random();

        public SmellyBubble(Fish hal)
        {
            this.Hala = hal;
            string musicpath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Music", "bubble.wav");
            this.punch = new AudioFileReader(musicpath);
            punchout = new WaveOut();
            punchout.Init(punch);
            string hitpath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Music", "hit.wav");
            this.hit = new AudioFileReader(hitpath);
            hitout = new WaveOut();
            hitout.Init(hit);
            this.Nev = "Smelly Bubble";
            this.Energiakoltseg = 2;
        }

        public override void Tamad(Fish mit, ObservableCollection<Fish> halak)
        {
            Hala.elfoglalt = true;

            if (!mit.meghalt && mit.Eleresiut != System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "fishbone.png"))
            {
                //  Task elso = new Task(() =>
                // {
                Csinal(mit);
                //  });
                //  elso.Start();

                Task vegzo = new Task(() =>
                {
                    //    elso.Wait();
                    Hala.elfoglalt = false;
                });
                vegzo.Start();
            }
            else
            {
                int index = 0;
                Hala.elfoglalt = true;
                for (int i = 0; i < halak.Count; i++)
                {
                    if (halak[i].meghalt == false)
                    {
                        index = i;
                    }
                }
                Task elso = new Task(() =>
                {
                    Csinal(halak[index]);
                });
                elso.Start();
                Task vegzo = new Task(() =>
                {
                    elso.Wait();
                    Hala.elfoglalt = false;
                    Hala.tamad = false;
                });
                vegzo.Start();
            }
        }
        public void Csinal(Fish mit)
        {
            Hala.lovedeke.aktiv = true;

            //  int szamlalo = 0;


            // while (szamlalo != 2)
            // {

            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(1);

            dt.Tick += delegate
            {
                if (Hala.lovedeke.aktiv)
                {


                    int szamlalo = 0;
                    while (szamlalo < 10)
                    {
                        if (Hala.lovedeke.x > mit.x)
                        {
                            Hala.lovedeke.x--;
                        }
                        else if (Hala.lovedeke.x < mit.x)
                        {
                            Hala.lovedeke.x++;
                        }
                        if (Hala.lovedeke.y < mit.y)
                        {
                            Hala.lovedeke.y++;
                        }
                        else if (Hala.lovedeke.y > mit.y)
                        {
                            Hala.lovedeke.y--;
                        }
                        szamlalo++;
                    }

                }
                else
                {
                    punchout.Stop();

                    int krite = R.Next(1, 6);
                    if (krite != 2)
                    {
                        mit.Elet -= R.Next(Hala.Ero, Hala.Ero + 8);
                    }
                    else
                    {
                        mit.Elet -= R.Next(2 * Hala.Ero, 2 * Hala.Ero + 8);
                    }


                    if (mit.Elet <= 0)
                    {
                        mit.meghalt = true;
                        mit.Eleresiut = Fishbone.AddFishbonepath();
                    }
                    Hala.lovedeke.aktiv = false;
                    Hala.lovedeke.x = Hala.x;
                    Hala.lovedeke.y = Hala.y;
                }

            };
            punch.CurrentTime = new TimeSpan(0L);
            punchout.Play();
            dt.Start();


            Task ta = new Task(() =>
            {
                while (Hala.lovedeke.aktiv) { } //Várakozik
                dt.Stop();
                hit.CurrentTime = new TimeSpan(0L);
                hitout.Play();
                mit.Eleresiut = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "bubblehit.png");
                Thread.Sleep(100);
                // hal.Eleresiutatcserel(regi);



            }, TaskCreationOptions.LongRunning);
            ta.Start();

            Task csere = new Task(() =>
            {
                //t.Wait();
                Thread.Sleep(80);
                // hal.Eleresiut = regi;
                if (!mit.meghalt)
                {
                    mit.Eleresiut = mit.regieleres;
                }
                else
                {
                    mit.Eleresiut = mit.dead;
                }



            }, TaskCreationOptions.LongRunning);
            csere.Start();

            // Hala.lovedeke.aktiv = true;


        }

        public override void SzovegLeszed()
        {
            throw new NotImplementedException();
        }
    }
}
//}
