using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_RPG.Models
{
    public class User
    {
        public int id { get; set; }
        public string username { get; set; } = "";
        public byte[] passwordHash { get; set; } = new Byte[0];
        public byte[] passwordSalt { get; set;} = new Byte[0];

        public List<Character>? characters;
    }
}