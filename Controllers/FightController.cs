using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DOTNET_RPG.DTOs.Fight;
using Microsoft.AspNetCore.Mvc;

namespace DOTNET_RPG.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FightController : ControllerBase
    {
        private readonly IFightService _fightService;

        public FightController(IFightService fightService)
        {
            _fightService = fightService;
        }

        [HttpPost("api/attack/weapon")]
        public async Task<ActionResult<ServiceResponse<AttackResultDTO>>> AttackWeapon(AttackBasicDTO combatInfo){
            var response = await _fightService.Attack(combatInfo);
            if (response.data == null)
                return BadRequest(response);
            return Ok(response);
            
        }

        [HttpPost("api/attack/skill")]
        public async Task<ActionResult<ServiceResponse<AttackResultDTO>>> AttackSkill(AttackSkillDTO combatInfo){
            var response = await _fightService.AttackWithSkill(combatInfo);
            if (response.data == null)
                return BadRequest(response);
            return Ok(response);
            
        }

        [HttpPost("api/attack/attackwithid")]
        public async Task<ActionResult<ServiceResponse<AttackResultDTO>>> DeathMatch(int[] characterIDs){
            var response = await _fightService.AutomaticFight(characterIDs);
            if (response.data == null)
                return BadRequest(response);
            return Ok(response);
        }

                [HttpPost("api/attack/attackall")]
        public async Task<ActionResult<ServiceResponse<AttackResultDTO>>> DeathMatch_all(){
            var response = await _fightService.AutomaticFightAll();
            if (response.data == null)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("attack/Leaderboards")]
        public async Task<ActionResult<ServiceResponse<LeaderboardDTO>>> GetLeaderboard(){
            return Ok(_fightService.GetLeaderboard());
        }
    }
}