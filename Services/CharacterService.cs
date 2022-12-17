using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DOTNET_RPG.DTOs.Character;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using DOTNET_RPG.Data;
using Microsoft.EntityFrameworkCore;

namespace DOTNET_RPG.Services
{
    /**
        Responsible for for all "communication" between the character model ->  and the database, and the client.
        That means it's a form of Repository.
    */
    public class CharacterService : ICharacterService
    {

        private IMapper _mapper;
        private DataContext _context;

        //DEPENDENCY INJECTION
        public CharacterService(IMapper mapper, DataContext context){
            _mapper = mapper;
            _context = context;
        }

        //FUNCS//

        public async Task<ServiceResponse<List<GetCharacterDTO>>> addCharacter(AddCharacterDTO newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            
            Character character = _mapper.Map<Character>(newCharacter); //convert characterDTO to Character.
            _context.characters.Add(character);
            await _context.SaveChangesAsync();
            serviceResponse.data = await _context.characters
                                        .Select(c => _mapper.Map<GetCharacterDTO>(c))
                                        .ToListAsync<GetCharacterDTO>();
            return serviceResponse;
        }


        public async Task<ServiceResponse<List<GetCharacterDTO>>> getAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            var dbCharacters = await _context.characters.ToListAsync();
            serviceResponse.data = dbCharacters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList<GetCharacterDTO>();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> getCharacterByID(int id)
        {   //can return null
            var serviceResponse = new ServiceResponse<GetCharacterDTO>();
            try
            {
                var dbCharacter = await _context.characters.FirstOrDefaultAsync(c => c.id == id) ?? throw new Exception(); //?? -> an operand where if the value returned is null, the right side of the ?? is evaluated.
                serviceResponse.data = _mapper.Map<GetCharacterDTO>(dbCharacter); //convert Character object, to GetCharacterDTO object.
                
            }
            //if no such ID exists
            catch(Exception)
            {
                serviceResponse.message = "ID does not exist in DB.";
                serviceResponse.success = false;
            }
 
            return serviceResponse;
        }



       public async Task<ServiceResponse<GetCharacterDTO>> updateCharacter(UpdateCharacterDTO updatedCharacterDTO)
       {
           var serviceResponse = new ServiceResponse<GetCharacterDTO>();
           //find the specific character to update, if not found, send an error report and update nothing.
           try
           {
             var dbCharacter = await _context.characters.FirstAsync(c => c.id == updatedCharacterDTO.id);
             dbCharacter =  _mapper.Map(updatedCharacterDTO, dbCharacter);

             await _context.SaveChangesAsync();
             serviceResponse.data = _mapper.Map<GetCharacterDTO>(dbCharacter);
           }
           catch (Exception e)
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
                Character character = await _context.characters.FirstAsync(c => c.id == id);
                _context.characters.Remove(character);
                await _context.SaveChangesAsync();

                serviceResponse.data = await _context.characters.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToListAsync();
            }
            catch (Exception e)
            {
                serviceResponse.message = e.Message+" I.E: ID is most likely wrong.";
                serviceResponse.success = false;
            }
            
            return serviceResponse;
        }
    }
}