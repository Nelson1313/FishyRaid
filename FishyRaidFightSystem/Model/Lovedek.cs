using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FishyRaidFightSystem.Model
{
    public class Lovedek
    {
        public double x { get; set; }

        public double y { get; set; }

        public bool aktiv { get; set; }

        public string eleres { get; set; }

        public Vector Sebesseg { get; set; }

        public Lovedek(double x, double y, string eleresiut)
        {
            this.x = x;
            this.y = y;
            this.eleres = eleresiut;
            this.aktiv = false;
        }

    }
}
