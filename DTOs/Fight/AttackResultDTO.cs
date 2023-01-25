using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_RPG.DTOs.Fight
{
    public class AttackResultDTO
    {
        public string attacker { get; set; } = "";
        public string opponent { get; set; } = "";
        public int attackerHP { get; set; } 
        public int opponentHP { get; set; } 
        public int damage { get; set; } 
    
    }
}