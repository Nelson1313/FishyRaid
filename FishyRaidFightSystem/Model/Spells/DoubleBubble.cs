using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FishyRaidFightSystem.Model.Spells
{
    public class DoubleBubble : Spell
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

        public DoubleBubble(Fish hal)
        {
            this.Hala = hal;
            string bubble= System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Music", "bubble.wav");
            this.punch = new AudioFileReader(bubble);
            punchout = new WaveOut();
            punchout.Init(punch);
            string hitpath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Music", "hit.wav");
            this.hit = new AudioFileReader(hitpath);
            hitout = new WaveOut();
            hitout.Init(hit);
            this.Nev = "Smelly Bubble";
            this.Energiakoltseg = 3;
        }

        public override void Tamad(Fish mit, ObservableCollection<Fish> halak)
        {
            if (!mit.meghalt && mit.Eleresiut != System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "fishbone.png"))
            {
                Hala.elfoglalt = true;
                Task elso = new Task(() =>
                {
                    Csinal(mit);
                });
                elso.Start();

                Task masodik = new Task(() =>
                {

                    elso.Wait();
                    Thread.Sleep(2000);
                    if (mit.Eleresiut != System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "fishbone.png"))
                    {
                        Csinal(mit);
                    }
                });
                masodik.Start();
                Task vegzo = new Task(() =>
                {
                    masodik.Wait();
                    if (Hala != null)
                    {
                        Hala.elfoglalt = false;
                    }
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

                Task masodik = new Task(() =>
                {

                    elso.Wait();
                    Thread.Sleep(2000);
                    if (halak[index].Eleresiut != System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "fishbone.png"))
                    {
                        Csinal(halak[index]);
                    }
                });
                masodik.Start();

                Task vegzo = new Task(() =>
                {
                    masodik.Wait();
                    if (Hala != null)
                    {
                        Hala.elfoglalt = false;
                    }
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


            Task t = new Task(() =>
            {

                punch.CurrentTime = new TimeSpan(0L);
                punchout.Play();
                int szamlalo = 0;
                while (Hala.lovedeke.aktiv)
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

                    //   szamlalo++;

                    //  if (szamlalo == 2)
                    //  {

                    szamlalo++;
                    if (szamlalo == 1)
                    {
                        Thread.Sleep(1);
                        szamlalo = -1;
                    }
                    //   szamlalo = 0;
                    //  }
                }
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
            });
            t.Start();

            Task ta = new Task(() =>
            {
                while (Hala.lovedeke.aktiv) { } //Várakozik
                hit.CurrentTime = new TimeSpan(0L);
                hitout.Play();
                //   mit.Eleresiut = "bubblehit.png";
                string hitpath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "bubblehit.png");
                mit.Eleresiut = hitpath;
                Thread.Sleep(100);
                // hal.Eleresiutatcserel(regi);



            }, TaskCreationOptions.LongRunning);
            ta.Start();

            Task csere = new Task(() =>
            {
                t.Wait();
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
