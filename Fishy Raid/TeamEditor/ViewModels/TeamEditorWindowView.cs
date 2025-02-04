using FishyRaidFightSystem.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TeamEditor.Logic;

namespace TeamEditor.ViewModels
{
    public class TeamEditorWindowView:ObservableRecipient
    {
        public ObservableCollection<Fish> FishesAboutToFight { get; set; }
        public ObservableCollection<Fish> AllFishes { get; set; }
        private Fish selectedFromAllFishes;

        public Fish SelectedFromAllFishes
        {
            get { return selectedFromAllFishes; }
            set {
                SetProperty(ref selectedFromAllFishes,value);
                (AddToTeam as RelayCommand).NotifyCanExecuteChanged();
                (RemoveFromTeam as RelayCommand).NotifyCanExecuteChanged();
            }
        }
        private Fish selectedFromFishesAboutToFight;

        public Fish SelectedFromFishesAboutToFight
        {
            get { return selectedFromFishesAboutToFight; }
            set {
                SetProperty(ref selectedFromFishesAboutToFight,value);
                (RemoveFromTeam as RelayCommand).NotifyCanExecuteChanged();             
            }
        }

        public ITeamEditorLogic logic { get; set; }
        public ICommand AddToTeam { get; set; }
        public ICommand RemoveFromTeam { get; set; }
        private Player Player;
        public ICommand Save { get; set; }
        public TeamEditorWindowView(ITeamEditorLogic logic)
        {
            this.logic = logic;
            AllFishes = new ObservableCollection<Fish>();
            FishesAboutToFight = new ObservableCollection<Fish>();

            Player = new Player();
            string filePath=Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName, "player.json");
            this.Player = (Player)SaveAndReadPlayer.Read(typeof(Player),filePath);
            

            AllFishes = Player.AllFishes;

            foreach (var item in Player.AllFishes)
            {
                item.Levelformat = item.EXP +"/"+ item.Level * 100;
            }
            foreach (var item in Player.FishesInFight)
            {
                item.Levelformat = item.EXP + "/" + item.Level * 100;
            }

            string path = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "1.png");
           // AllFishes.Add(new Fish() { Elet = 100, sorszam = 3, Eleresiut = path, regieleres = path, pozicio = 20, Kozelsebzes = 10, Helye = 3 });
            FishesAboutToFight = Player.FishesInFight;
            ;
            logic.Setup(AllFishes,FishesAboutToFight);

            AddToTeam = new RelayCommand(
                ()=>logic.AddToTeam(SelectedFromAllFishes),
                ()=>SelectedFromAllFishes!=null
                );
            RemoveFromTeam = new RelayCommand(
                () => logic.RemoveFromTeam(SelectedFromFishesAboutToFight),
                () => SelectedFromFishesAboutToFight != null
                ) ;
            Save = new RelayCommand(
                () => logic.Save(ref this.Player)
                );
            Messenger.Register<TeamEditorWindowView, string, string>(this, "TeamInfo", (recipient,msg)=> 
            {
                OnPropertyChanged("Fish Added");
                OnPropertyChanged("Fish Removed");
            });
        }
        
        public TeamEditorWindowView()
            :this(Ioc.Default.GetService<ITeamEditorLogic>())
        {

        }



    }
}
