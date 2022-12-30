using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DOTNET_RPG.DTOs.Character;
using DOTNET_RPG.DTOs.WeaponDTO;

namespace DOTNET_RPG.Services
{
    public interface ICharacterService
    {
        Task <ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters();

        Task<ServiceResponse<GetCharacterDTO>> GetCharacterByID(int id);

        Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newCharacter);

        Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter (UpdateCharacterDTO updatedCharacter);

        Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter (int id);

        Task<ServiceResponse<GetCharacterDTO>> AddWeapon (int characterID, int weaponID);
        
    }
}