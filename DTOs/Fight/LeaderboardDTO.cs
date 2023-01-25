using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_RPG.DTOs.Fight
{
    public class LeaderboardDTO
    {
        public string name { get; set; }
        public int victories { get; set; }
        public int defeats { get; set; }
        public int fights   {get; set; }
    }
}