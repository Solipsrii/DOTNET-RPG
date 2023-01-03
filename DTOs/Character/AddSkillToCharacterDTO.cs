using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_RPG.DTOs.Character
{
    public class AddSkillToCharacterDTO
    {
        public int characterID { get; set; }
        public int skillID { get; set; }
    }
}