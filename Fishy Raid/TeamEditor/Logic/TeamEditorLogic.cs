using FishyRaidFightSystem.Model;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TeamEditor.Logic
{
    public class TeamEditorLogic : ITeamEditorLogic
    {
        IList<Fish> allfish;
        IList<Fish> teamfishes;
        IMessenger messenger;
        public TeamEditorLogic(IMessenger messenger)
        {
            this.messenger = messenger;
        }
        public void AddToTeam(Fish selectedFish) //Lehet, hogy megőrírt a halott enemyt
        {
            bool helyegy = false;
            bool helyketto = false;
            bool helyharom = false;
            foreach (var item in teamfishes)
            {
                if (item.Helye == 1)
                {
                    helyegy = true;
                }
                else if (item.Helye == 2)
                {
                    helyketto = true;
                }
                else if (item.Helye == 3)
                {
                    helyharom = true;
                }
            }

            if (helyegy==false)
            {
                selectedFish.Helye = 1;
              //  selectedFish.sorszam = 1;
                selectedFish.pozicio = 0;
                selectedFish.sorszam = 1; //1
            }
            else if (helyketto==false)
            {
                selectedFish.Helye = 2;
             //   selectedFish.sorszam = 2;
                selectedFish.pozicio = 30;
                selectedFish.sorszam = 2; //3
            }
            else if (helyharom==false)
            {
                selectedFish.Helye = 3;
               // selectedFish.sorszam = 3;
                selectedFish.pozicio = 20;
                selectedFish.sorszam = 3; //2
            }


            if (teamfishes.Count<3)
            {                
                teamfishes.Add(selectedFish);
                messenger.Send("Fish Added", "TeamInfo");
                allfish.Remove(selectedFish);
                messenger.Send("Fish Removed","TeamInfo");
            }
            else
            {
                MessageBox.Show("You can only take 3 Fish to Battle!");
            }            
        }

        

        public void RemoveFromTeam(Fish selectedFish)
        {
            teamfishes.Remove(selectedFish);
            messenger.Send("Fish Removed","TeamInfo");
            allfish.Add(selectedFish);
            messenger.Send("Fish Added", "TeamInfo");
        }
        public void Save(ref Player p) 
        {
            if (teamfishes.Count == 3)
            {
                string filePath= Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName, "player.json");

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
                }
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                SaveAndReadPlayer.Save(p,filePath);
                MessageBox.Show("Team Saved!");
            }
            else
            {
                MessageBox.Show("You need to have 3 Fishes in the Team to Save!");
            }

        }
        public void Setup(IList<Fish> allfish, IList<Fish> teamfishes)
        {
            this.allfish = allfish;
            this.teamfishes = teamfishes;
        }
    }
}
