using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DOTNET_RPG.DTOs.Skills;
using DOTNET_RPG.DTOs.WeaponDTO;
using DOTNET_RPG.Models;

namespace DOTNET_RPG.DTOs.Character
{
    public class GetCharacterDTO
    {
        public int id { get; set; }
        public string name { get; set; } = "Frodo";
        public int hitPoints { get; set; } = 100;

        public int strength { get; set; } = 10;
        
        public int defense { get; set; } = 10;

        public int intelligence { get; set; } = 10;

        public GetWeaponDTO weapon { get; set; }

        public List<GetSkillDTO> skills {get; set;}

        public RpgClass Class { get; set; } = RpgClass.Mage;

        public int fights { get; set; }
        public int victories { get; set; }
        public int defeats { get; set; }
    }
}