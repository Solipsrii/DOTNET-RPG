using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DOTNET_RPG.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace DOTNET_RPG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRep;
        public AuthController(IAuthRepository authRep)
        {
            _authRep = authRep;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> register (UserRegisterDTO request){
            var response = await _authRep.register(
                new User {username = request.username}, //username
                          request.password);            //password

            //note: we use User in AuthRepository's register(), for future-proofing. Maybe add emails and such. Tho then we'll also use an extra method to register all of the reqests.
            return returnHandling<int>(response);
        }

        [HttpPost("Login")]
       public async Task<ActionResult<ServiceResponse<string>>> login (UserLoginDTO request){
            var response = await _authRep.login(request.username, request.password);
            return returnHandling<string>(response);
       }

    /*
    ############################################################################################################################################
        ## HELPER METHODS ##  ## HELPER METHODS ##  ## HELPER METHODS ##  ## HELPER METHODS ##  ## HELPER METHODS ##  ## HELPER METHODS ##  
    ############################################################################################################################################
    */

        private ActionResult<ServiceResponse<T>> returnHandling<T> (ServiceResponse<T> response){
            if (!response.success)
                return BadRequest(response);

            return response;
        }
    }
}