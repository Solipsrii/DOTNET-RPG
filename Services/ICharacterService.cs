using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DOTNET_RPG.DTOs.Character;

namespace DOTNET_RPG.Services
{
    public interface ICharacterService
    {
        Task <ServiceResponse<List<GetCharacterDTO>>> getAllCharacters();

        Task<ServiceResponse<GetCharacterDTO>> getCharacterByID(int id);

        Task<ServiceResponse<List<GetCharacterDTO>>> addCharacter(AddCharacterDTO newCharacter);

        Task<ServiceResponse<GetCharacterDTO>> updateCharacter (UpdateCharacterDTO updatedCharacter);

        Task<ServiceResponse<List<GetCharacterDTO>>> deleteCharacter (int id);
    }
}