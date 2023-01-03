using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DOTNET_RPG.DTOs.Character;
using DOTNET_RPG.DTOs.Skills;
using DOTNET_RPG.DTOs.WeaponDTO;

namespace DOTNET_RPG
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //<source, destination>
            CreateMap<Character, GetCharacterDTO>();
            CreateMap<AddCharacterDTO, Character>();
            CreateMap<UpdateCharacterDTO, Character>();
            CreateMap<AddWeaponDTO, Weapon>();
            CreateMap<GetWeaponDTO, Weapon>();
            CreateMap<Weapon, AddWeaponDTO>();
            CreateMap<Weapon, GetWeaponDTO>();

            CreateMap<Skill, GetSkillDTO>();
        }
    }
}