using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FishyRaidHelpers
{
    public class Fish
    {
        public int Elet { get; set; }
        public string Eleresiut { get; set; }
        public int sorszam { get; set; }
        public int pozicio { get; set; }
        bool novelbvagycsokkent;
        public bool meghalt { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double oldx { get; set; }
        public double oldy { get; set; }
        public Fish Hala { get; set; }
        public bool tamad { get; set; }
        public bool visszamegy { get; set; }
        public int Kozelsebzes { get; set; }
        public int Helye { get; set; }
        public Fish()
        {
            this.novelbvagycsokkent = true;
            this.meghalt = false;
            this.x = 0;
            this.y = 0;
            this.tamad = false;
            this.visszamegy = false;

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
                            if (mit.Elet <= 0)
                            {
                                mit.Elet = 0;
                                mit.meghalt = true;
                                mit.Eleresiut = "fishbone.png";
                            }
                            sebzett = true;

                        }

                        if ((int)x == (int)oldx && (int)y == (int)oldy)
                        {
                            this.tamad = false;
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

        public override string ToString()
        {
            string datasToSave="";
            foreach (var item in this.GetType().GetProperties())
            {
                if (item.GetType().Name.Equals(Hala))
                {
                    datasToSave += "/"+item.GetType().ToString()+"/";
                }
                else
                {
                    datasToSave += item.GetType().GetFields() + "#";
                }
               
            }

            return datasToSave;
        }
    }
}
