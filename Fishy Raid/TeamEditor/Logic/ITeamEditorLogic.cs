using FishyRaidFightSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamEditor.Logic
{
    public interface ITeamEditorLogic
    {
        void AddToTeam(Fish selectedFish);
        void RemoveFromTeam(Fish selectedFish);
        void Setup(IList<Fish> allfish, IList<Fish> teamfishes);
        void Save(ref Player p);
    }
}
