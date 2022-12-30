using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DOTNET_RPG.Models.Enums;

namespace DOTNET_RPG.Models
{
    public class Character
    {
        public int id { get; set; }
        public string name { get; set; } = "Frodo";
        public int hitPoints { get; set; } = 100;

        public int strength { get; set; } = 10;
        
        public int defense { get; set; } = 10;

        public int intelligence { get; set; } = 10;

        public RpgClass Class { get; set; } = RpgClass.Mage;

        public User? user { get; set; }
        public Weapon? weapon { get; set; } = new Weapon(); //startout weapon as null. not sure how to fix it to default to = new Weapon();.

    }
}