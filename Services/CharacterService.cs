using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DOTNET_RPG.DTOs.Character;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace DOTNET_RPG.Services
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characterList = new List<Character> {
            new Character {},
            new Character {id = 1, name = "Sam", strength = 15, Class = RpgClass.Cleric}
            };

        private IMapper _mapper;

        //DEPENDENCY INJECTION
        public CharacterService(IMapper mapper){
              this._mapper = mapper;
        }

        //FUNCS//

        public async Task<ServiceResponse<List<GetCharacterDTO>>> addCharacter(AddCharacterDTO newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            Character character = _mapper.Map<Character>(newCharacter); //convert characterDTO to Character.
            character.id = characterList.Max(c => c.id) + 1; //attain max ID.

            characterList.Add(character); 
            serviceResponse.data = characterList.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList<GetCharacterDTO>();
            return serviceResponse;
        }


        public async Task<ServiceResponse<List<GetCharacterDTO>>> getAllCharacters()
        {
            return new ServiceResponse<List<GetCharacterDTO>> {data = characterList.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList<GetCharacterDTO>()};
        }

        public async Task<ServiceResponse<GetCharacterDTO>> getCharacterByID(int id)
        {   //can return null
            var serviceResponse = new ServiceResponse<GetCharacterDTO>();
            var character = characterList.FirstOrDefault(c => c.id == id);
            serviceResponse.data = _mapper.Map<GetCharacterDTO>(character); //convert Character object, to GetCharacterDTO object.
            return serviceResponse;
        }



       public async Task<ServiceResponse<GetCharacterDTO>> updateCharacter(UpdateCharacterDTO updatedCharacterDTO)
       {
           var serviceResponse = new ServiceResponse<GetCharacterDTO>();
           //find the specific character to update, if not found, send an error report and update nothing.
           try
           {
             Character character = characterList.Find(c => c.id == updatedCharacterDTO.id);
             character = _mapper.Map(updatedCharacterDTO, character);
             serviceResponse.data = _mapper.Map<GetCharacterDTO>(character);
           }
           catch (ArgumentNullException e)
           {
            serviceResponse.message = e.Message;
            serviceResponse.success = false;
           }
           //using auto-mapper do do a deep-copy from one object to another.
           return serviceResponse;
       }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> deleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDTO>> serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();

            try 
            {
                Character character = characterList.First(c => c.id == id);
                characterList.Remove(character);
                serviceResponse.data = characterList.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();
            }
            catch (Exception e)
            {
                serviceResponse.message = e.Message;
                serviceResponse.success = false;
            }

            return serviceResponse;
        }
    }
}