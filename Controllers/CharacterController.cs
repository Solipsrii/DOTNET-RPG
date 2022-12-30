using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DOTNET_RPG.Services;
using DOTNET_RPG.DTOs.Character;
using System.Security.Claims;

namespace DOTNET_RPG.Controllers
{
    //ATTRIBUTES:
    [Authorize]
    [ApiController] //Attribute-based routing! ApiController Adds stuff to make this controller more conveinent for web use. How? Why?
    [Route("api/[controller]")] //allows us to type api/Character... to get the data we want. Service calls. [controller] being the "convention" of the class name, being CharacterController, C# knows to eliminate Controller, so we can access this controller, by writing: /api/Character.
    public class CharacterController : ControllerBase
    {
        //_<name> : injected object.
        private readonly ICharacterService _characterService;
        
        //point of DEPENDENCY INJECTION.  
        public CharacterController(ICharacterService characterService){
            this._characterService = characterService;
        }
        
           

        //IActionresult, returns a status code to the client. OK being a "200ok" stauts code, in which we return all characters registered to a specific User, see class Character.
        [HttpGet("GetAll")]
        // or can use in addition: [Route("GetAll")]
        //HttpGet -- marks for Swagger, and in general for any web-API, that this function is a "get" function, and should be retrieved on any get-request.
        public async Task<ActionResult<List<ServiceResponse>>> Get(){
                return Ok(await _characterService.GetAllCharacters());
        }

        
        [HttpGet("{id}")] //id matches the parameter in getSingle(int id)!
        public async Task<ActionResult<ServiceResponse>> getSingle(int id){
            return Ok(await _characterService.GetCharacterByID(id));
        }

        [HttpPost("Add")]
        public async Task<ActionResult<List<ServiceResponse>>> addCharacter (AddCharacterDTO character){
            return Ok(await _characterService.AddCharacter(character));
        }

        [HttpPut("AddWeaponToCharacter")]
        public async Task<ActionResult<ServiceResponse>> addWeaponToChar (int characterID, int weaponID){
            var response = await _characterService.AddWeapon(characterID, weaponID);
            if (response == null)
                return BadRequest(response);
            
            return Ok(response);
        }

        [HttpPut("Update")]
        public async Task<ActionResult<List<ServiceResponse>>> updateCharacter (UpdateCharacterDTO updatedCharacter){
            var serviceResponse = await _characterService.UpdateCharacter(updatedCharacter);
            if (serviceResponse.data == null)
                return NotFound(serviceResponse);

            return Ok(serviceResponse);
        }

        
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<ServiceResponse>>> deleteCharacter (int id){
            var serviceResponse = await _characterService.DeleteCharacter(id);
            if (serviceResponse.data == null)
                return NotFound(serviceResponse);

            return Ok(serviceResponse);
        }

    /*
    ############################################################################################################################################
        ## HELPER METHODS ##  ## HELPER METHODS ##  ## HELPER METHODS ##  ## HELPER METHODS ##  ## HELPER METHODS ##  ## HELPER METHODS ##  
    ############################################################################################################################################
    */

    }
}