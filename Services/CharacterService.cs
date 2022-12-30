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
using System.Security.Claims;
using DOTNET_RPG.DTOs.WeaponDTO;

namespace DOTNET_RPG.Services
{
    /**
        Responsible for for all "communication" between the character model ->  and the database, and the client.
        That means it's a form of Repository.
    */
    public class CharacterService : ICharacterService
    {

        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        //DEPENDENCY INJECTION
        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor){
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        //FUNCS//

        public async Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            
            Character character = _mapper.Map<Character>(newCharacter); //convert characterDTO to Character.
            character.user = await _context.users.FirstOrDefaultAsync(u => u.id == GetUserID()); //add the new character to the currently authenticatd user. Note: this system is pretty dumb, as if you can add a character, that means you have to log in, so "looking for the user in the db" is pretty dumb.
            
            _context.characters.Add(character); //add the character to the general DBSet
            await _context.SaveChangesAsync();

            //prep the serviceResponse with the list of all characters, in the form of GetCharacterDTO.
            serviceResponse.data = await _context.characters
                                        .Select(c => _mapper.Map<GetCharacterDTO>(c))
                                        .ToListAsync<GetCharacterDTO>();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            int userID = GetUserID();
            var dbCharacters = await _context.characters
                .Include(c => c.user)
                .Include(c => c.weapon)
                .Where(c => c.user.id == userID)
                .ToListAsync();
            
            serviceResponse.data = dbCharacters
                .Select(c => _mapper.Map<GetCharacterDTO>(c))
                .ToList<GetCharacterDTO>();
                
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> GetCharacterByID(int id)
        {   //can return null
            var serviceResponse = new ServiceResponse<GetCharacterDTO>();
            try
            {
                var dbCharacter = await _context.characters.FirstOrDefaultAsync(c => (c.id == id) && c.user.id == GetUserID()) ?? throw new Exception(); //?? -> an operand where if the value returned is null, the right side of the ?? is evaluated.
                serviceResponse.data = _mapper.Map<GetCharacterDTO>(dbCharacter); //convert Character object, to GetCharacterDTO object.
                
            }
            //if no such ID exists
            catch(Exception)
            {
                serviceResponse.message = "ID does not exist in DB or character doesn't exist in user's list.";
                serviceResponse.success = false;
            }
 
            return serviceResponse;
        }



       public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updatedCharacterDTO)
       {
           var serviceResponse = new ServiceResponse<GetCharacterDTO>();
           //find the specific character to update, if not found, send an error report and update nothing.
           try
           {
             var dbCharacter = await _context.characters.FirstAsync(c => (c.id == updatedCharacterDTO.id) && (c.user.id == GetUserID())); //returns a Character if exists
             dbCharacter =  _mapper.Map(updatedCharacterDTO, dbCharacter); //

             await _context.SaveChangesAsync();
             //using auto-mapper do do a deep-copy from one object to another.
             serviceResponse.data = _mapper.Map<GetCharacterDTO>(dbCharacter);
           }
           catch (Exception e)
           {
            serviceResponse.message = e.Message;
            serviceResponse.success = false;
           }
           
           return serviceResponse;
       }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDTO>> serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();

            try 
            {
                Character character = await _context.characters.FirstAsync(c => (c.id == id) && (c.user.id == GetUserID())) ?? throw new Exception("No such character ID, or no access to it.");
                _context.characters.Remove(character);
                await _context.SaveChangesAsync();

                serviceResponse.data = await _context.characters
                 .Where(c => c.user.id == GetUserID()) //filter
                 .Select(c => _mapper.Map<GetCharacterDTO>(c)) //manipulate every Where result. Convert Character to Get.
                 .ToListAsync();
                serviceResponse.message = "Successfully removed \""+character.name+"\".";
            }
            catch (Exception e)
            {
                serviceResponse.message = e.Message;
                serviceResponse.success = false;
            }
            
            return serviceResponse;
        }


        //############ WEAPONS STUFF HERE #################
        
        //add weapon to a particular character
        public async Task<ServiceResponse<GetCharacterDTO>> AddWeapon(int characterID, int weaponID)
        {
            var response = new ServiceResponse<GetCharacterDTO>();
            int userID = GetUserID();
            var characterList = await _context.characters
                                .Where(c => c.user.id == userID)
                                .ToListAsync();

            if (characterList.Count == 0)
                response.setErrorMessage("First add a character before you try to add it a weapon!");
            
            //a character exists
            else
            {
                //get list of all characters the user owns, and then check if charId or weapId are correct.
                var character = characterList.FirstOrDefault(c => c.id == characterID);
                if (character == null)
                    response.setErrorMessage("Tried to add a weapon to a non-existing character-ID.");

                else //can add the weapon to the given character. that means adding via the id or the name.
                {
                    //maybe alter this method so it also accepts CheckWeaponDTO, to allow name or id as input? The "or" here is difficult.
                    if (await WeaponExists(weaponID))
                        {
                            var weapon = await _context.weapons.FirstOrDefaultAsync(w => w.id == weaponID);
                            character.weapon = weapon;
                            await _context.SaveChangesAsync();
                        }
                    else
                        response.setErrorMessage("Wrong weapon ID.");
                }
                response.data = _mapper.Map<GetCharacterDTO>(character);
            }

            return response;
        }



        //#############  HELPER METHODS ###################

        //get the authenticated user's ID
        private int GetUserID() => int.Parse(_httpContextAccessor.HttpContext.User.
                                             FindFirstValue(ClaimTypes.NameIdentifier));

        private async Task<bool> CharacterExists(int characterID){
            return(await _context.characters.AnyAsync(c => c.id == characterID));
        }
        private async Task<bool> WeaponExists(int weaponID){
            //if ID exists in db
            //TODO: Extend to CheckWeaponDTO to also check if either the ID or the names are correct?
            return (await _context.weapons.AnyAsync(w => (w.id == weaponID)));
        }

    }
}