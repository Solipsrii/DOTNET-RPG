using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_RPG.Models
{
    public class Skill
    {
        public int id { get; set; }
        public string name { get; set; } = "";
        public int damage { get; set; }
        public List<Character> Characters { get; set; } //many-to-many relationship in EF
    }
}