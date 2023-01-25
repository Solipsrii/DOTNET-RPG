using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DOTNET_RPG.DTOs.Fight;
using Microsoft.AspNetCore.Mvc;

namespace DOTNET_RPG.Services
{
    public interface IFightService
    {
        Task<ServiceResponse<AttackResultDTO>> Attack(AttackBasicDTO combatInfo);
        Task<ServiceResponse<AttackResultDTO>> AttackWithSkill(AttackSkillDTO combatInfo);
        Task<ServiceResponse<AutomaticFightDTO>> AutomaticFight (int[] characterIDs);
        Task<ServiceResponse<AutomaticFightDTO>> AutomaticFightAll ();
        Task<ServiceResponse<List<LeaderboardDTO>>> GetLeaderboard();
    }
}