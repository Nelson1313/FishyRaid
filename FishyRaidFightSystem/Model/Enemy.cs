using FishyRaidFightSystem.Model.Spells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishyRaidFightSystem.Model
{
    public class Enemy
    {
        public ObservableCollection<Fish> FishesInFight { get; set; }
        public ObservableCollection<Fish> AllFishes { get; set; }
        public int Level { get; set; }
        public ObservableCollection<Potion> Potions { get; set; }
        public int Energy { get; set; }

        public Enemy()
        {
            this.AllFishes = new ObservableCollection<Fish>();
            this.FishesInFight = new ObservableCollection<Fish>();
            this.Energy = 3;           
        }

        public void EnemyLoad(string melyikpalya)
        {
            if (melyikpalya == "1")
            {
                string imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "jellyfish.png");
                this.FishesInFight = new ObservableCollection<Fish>();
                this.FishesInFight.Add(new Fish() { Elet = 60, sorszam = 1, Eleresiut = imgPath, regieleres = imgPath, pozicio = 10, Kozelsebzes = 10, Helye = 1, Level = 1, Maxhp = 60 });
                this.FishesInFight.Add(new Fish() { Elet = 60, sorszam = 2, Eleresiut = imgPath, regieleres = imgPath, pozicio = 23, Kozelsebzes = 10, Helye = 2, Level = 1, Maxhp = 60 });
                this.FishesInFight.Add(new Fish() { Elet = 60, sorszam = 3, Eleresiut = imgPath, regieleres = imgPath, pozicio = 30, Kozelsebzes = 10, Helye = 3, Level = 1, Maxhp = 60 });
                int szamlalo = 1;
                foreach (var item in this.FishesInFight)
                {

                    item.Tavolsagi = new RandomBubble(item);


                    szamlalo++;

                }
            }
            else if (melyikpalya == "2")
            {
                string imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "polip.png");
                this.FishesInFight = new ObservableCollection<Fish>();
                this.FishesInFight.Add(new Fish() { Elet = 160, sorszam = 1, Eleresiut = imgPath, regieleres = imgPath, pozicio = 10, Ero = 30, Kozelsebzes = 20, Helye = 1, Level = 4, Maxhp = 160 });
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "jellyfish.png");
                this.FishesInFight.Add(new Fish() { Elet = 120, sorszam = 2, Eleresiut = imgPath, regieleres = imgPath, pozicio = 23, Ero = 20, Kozelsebzes = 20, Helye = 2, Level = 4, Maxhp = 120 });
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "jellyfish.png");
                this.FishesInFight.Add(new Fish() { Elet = 120, sorszam = 3, Eleresiut = imgPath, regieleres = imgPath, pozicio = 30, Ero = 20, Kozelsebzes = 20, Helye = 3, Level = 4, Maxhp = 120 });
                foreach (var item in this.FishesInFight)
                {
                    item.Tavolsagi = new DoubleBubble(item);
                }
            }
            else if (melyikpalya == "3")
            {
                string imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "polip.png");
                this.FishesInFight = new ObservableCollection<Fish>();
                this.FishesInFight.Add(new Fish() { Elet = 160, sorszam = 1, Eleresiut = imgPath, regieleres = imgPath, Ero = 40, pozicio = 10, Kozelsebzes = 20, Helye = 1, Level = 1, Maxhp = 160 });
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "jellyfish.png");
                this.FishesInFight.Add(new Fish() { Elet = 140, sorszam = 2, Eleresiut = imgPath, regieleres = imgPath, Ero = 30, pozicio = 23, Kozelsebzes = 20, Helye = 2, Level = 1, Maxhp = 140 });
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "polip.png");
                this.FishesInFight.Add(new Fish() { Elet = 160, sorszam = 3, Eleresiut = imgPath, regieleres = imgPath, Ero = 40, pozicio = 30, Kozelsebzes = 20, Helye = 3, Level = 1, Maxhp = 160 });
                foreach (var item in this.FishesInFight)
                {
                    item.Tavolsagi = new DoubleBubble(item);
                }
                this.FishesInFight[1].Tavolsagi = new RandomBubble(this.FishesInFight[1]);
            }
            else if (melyikpalya == "4")
            {
                string imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "sponge.png");
                this.FishesInFight = new ObservableCollection<Fish>();
                this.FishesInFight.Add(new Fish() { Elet = 240, sorszam = 1, Eleresiut = imgPath, regieleres = imgPath, Ero = 50, pozicio = 10, Kozelsebzes = 60, Helye = 1, Level = 1, Maxhp = 240 });
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "polip.png");
                this.FishesInFight.Add(new Fish() { Elet = 160, sorszam = 2, Eleresiut = imgPath, regieleres = imgPath, Ero = 40, pozicio = 23, Kozelsebzes = 40, Helye = 2, Level = 1, Maxhp = 160 });
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "polip.png");
                this.FishesInFight.Add(new Fish() { Elet = 160, sorszam = 3, Eleresiut = imgPath, regieleres = imgPath, Ero = 40, pozicio = 30, Kozelsebzes = 40, Helye = 3, Level = 1, Maxhp = 160 });
                foreach (var item in this.FishesInFight)
                {
                    item.Tavolsagi = new RandomBubble(item);
                }
                this.FishesInFight[0].Tavolsagi = new BubbleKiss(this.FishesInFight[0]);
            }
            else if (melyikpalya == "5")
            {
                string imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "polip.png");
                this.FishesInFight = new ObservableCollection<Fish>();
                this.FishesInFight.Add(new Fish() { Elet = 360, sorszam = 1, Eleresiut = imgPath, regieleres = imgPath, Ero = 60, pozicio = 10, Kozelsebzes = 60, Helye = 1, Level = 1, Maxhp = 360 });
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "sponge.png");
                this.FishesInFight.Add(new Fish() { Elet = 160, sorszam = 2, Eleresiut = imgPath, regieleres = imgPath, Ero = 40, pozicio = 23, Kozelsebzes = 40, Helye = 2, Level = 1, Maxhp = 160 });
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "sponge.png");
                this.FishesInFight.Add(new Fish() { Elet = 360, sorszam = 3, Eleresiut = imgPath, regieleres = imgPath, Ero = 60, pozicio = 30, Kozelsebzes = 40, Helye = 3, Level = 1, Maxhp = 360 });
                foreach (var item in this.FishesInFight)
                {
                    item.Tavolsagi = new DoubleBubble(item);
                }
                this.FishesInFight[0].Tavolsagi = new RandomBubble(this.FishesInFight[0]);
                this.FishesInFight[2].Tavolsagi = new RandomBubble(this.FishesInFight[2]);
            }
            else if (melyikpalya == "6")
            {
                string imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "axolotl.png");
                this.FishesInFight = new ObservableCollection<Fish>();
                this.FishesInFight.Add(new Fish() { Elet = 540, sorszam = 1, Eleresiut = imgPath, regieleres = imgPath, Ero = 50, pozicio = 10, Kozelsebzes = 60, Helye = 1, Level = 1, Maxhp = 540 });
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "sponge.png");
                this.FishesInFight.Add(new Fish() { Elet = 360, sorszam = 2, Eleresiut = imgPath, regieleres = imgPath, Ero = 80, pozicio = 23, Kozelsebzes = 80, Helye = 2, Level = 1, Maxhp = 360 });
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "sponge.png");
                this.FishesInFight.Add(new Fish() { Elet = 360, sorszam = 3, Eleresiut = imgPath, regieleres = imgPath, Ero = 80, pozicio = 30, Kozelsebzes = 80, Helye = 3, Level = 1, Maxhp = 360 });

                this.FishesInFight[0].Tavolsagi = new BabyThrow(this.FishesInFight[0]);
                this.FishesInFight[1].Tavolsagi = new RandomBubble(this.FishesInFight[0]);
                this.FishesInFight[2].Tavolsagi = new RandomBubble(this.FishesInFight[2]);
            }
            else if (melyikpalya == "7")
            {
                string imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "axolotl.png");
                this.FishesInFight = new ObservableCollection<Fish>();
                this.FishesInFight.Add(new Fish() { Elet = 860, sorszam = 1, Eleresiut = imgPath, regieleres = imgPath, Ero = 50, pozicio = 10, Kozelsebzes = 60, Helye = 1, Level = 1, Maxhp = 540 });
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "sponge.png");
                this.FishesInFight.Add(new Fish() { Elet = 720, sorszam = 2, Eleresiut = imgPath, regieleres = imgPath, Ero = 80, pozicio = 23, Kozelsebzes = 80, Helye = 2, Level = 1, Maxhp = 360 });
                imgPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "axolotl.png");
                this.FishesInFight.Add(new Fish() { Elet = 860, sorszam = 3, Eleresiut = imgPath, regieleres = imgPath, Ero = 80, pozicio = 30, Kozelsebzes = 80, Helye = 3, Level = 1, Maxhp = 360 });

                this.FishesInFight[0].Tavolsagi = new BabyThrow(this.FishesInFight[0]);
                this.FishesInFight[1].Tavolsagi = new RandomBubble(this.FishesInFight[0]);
                this.FishesInFight[2].Tavolsagi = new BabyThrow(this.FishesInFight[2]);
            }

        }
    }
}
