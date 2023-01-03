using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DOTNET_RPG.DTOs.WeaponDTO;
using Microsoft.AspNetCore.Mvc;

namespace DOTNET_RPG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponService _weaponService;

        //DEPENCENY INJECTION
        public WeaponController(IWeaponService weaponService){
            _weaponService = weaponService;
        }

        [HttpPost("AddWeapon")]
        public async Task<ActionResult<ServiceResponse>> AddWeapon (AddWeaponDTO weapon){
            var response = await _weaponService.AddWeapon(weapon);

            if (response == null)
                return BadRequest(response);
            return Ok(response);
        }

         [HttpGet("GetWeaponsList")]
        public async Task<ActionResult<List<ServiceResponse>>> GetWeaponList (){
            return Ok(await _weaponService.GetWeaponList());
        }
    }
}