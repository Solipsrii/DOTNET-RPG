using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_RPG.DTOs.Fight
{
    public class AttackSkillDTO
    {
        public int attackerID { get; set; }
        public int opponentID { get; set; }

        public int skillID { get; set; }
    }
}