using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_RPG.DTOs.Character
{
    public class AddCharacterDTO
    {   
        public string name { get; set; } = "Frodo";
        public int hitPoints { get; set; } = 100;

        public int strength { get; set; } = 10;
        
        public int defense { get; set; } = 10;

        public int intelligence { get; set; } = 10;

        public RpgClass Class { get; set; } = RpgClass.Mage;
    }
}