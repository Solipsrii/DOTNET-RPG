using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_RPG.DTOs.WeaponDTO
{
    public class AddWeaponDTO
    {

        public string name { get; set; } = "fists";
        public int damage { get; set; } = 1;
    }
}