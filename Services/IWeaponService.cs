using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DOTNET_RPG.DTOs.Character;
using DOTNET_RPG.DTOs.WeaponDTO;

namespace DOTNET_RPG.Services
{
    public interface IWeaponService
    {
        public Task<ServiceResponse<List<GetWeaponDTO>>> addWeapon (AddWeaponDTO weapon);
    }
}