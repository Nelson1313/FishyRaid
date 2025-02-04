using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace FishyRaidHelpers
{
    public static class SaverOrReader
    {
        public static Player Read()
        {
            //JsonSerializer.Deserialize();

            Player p = new Player();
            string currentD = Directory.GetCurrentDirectory();
            string read=File.ReadAllText(currentD);
            string[] strng = read.Split('#');

            return p;
        }

        public static void Save(Player p)
        {
            JsonSerializer.Serialize(p);




            string toSave = p.ToString();
            string currentD = Directory.GetCurrentDirectory();
            File.WriteAllText(currentD,toSave);
        }

    }
}
