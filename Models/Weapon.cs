using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_RPG.Models
{
    public class Weapon
    {
        public Weapon(int id)
        {
            this.id = id;    
        }

        public int id { get; set; }
        public string name { get; set; } = "fists";
        public int damage { get; set; } = 1;
    }
}