using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DOTNET_RPG.DTOs.Fight;
using Microsoft.AspNetCore.Mvc;

namespace DOTNET_RPG.Services
{
    public class FightService : IFightService
    {
        private readonly DataContext _dataContext;

        public FightService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ServiceResponse<AttackResultDTO>> Attack(AttackBasicDTO combatInfo)
        {
            //get attacker / defender data
            var response = new ServiceResponse<AttackResultDTO>();
             try
            {
                var attacker = await _dataContext.characters
                                        .Include(c => c.weapon)
                                        .FirstOrDefaultAsync(c => c.id == combatInfo.attackerID)
                                            ?? throw new Exception("Wrong character ID for attacker.");

                var opponent = await _dataContext.characters
                                        .FirstOrDefaultAsync(c => c.id == combatInfo.opponentID)
                                            ?? throw new Exception("Wrong character ID for the opponent.");

                var rand = new Random();
                //dmg calculation : (weapon + random str) - (opponent defense's buff)
                int damage = GetWeaponDamage(attacker, opponent, rand);
                if (damage < 0) damage = 0;

                response.message = $"{attacker.name} attacks {opponent.name} with {attacker.weapon.name}, dealing {damage} damage!";

                opponent.hitPoints -= damage;
                if (opponent.hitPoints <= 0)
                {
                    response.message += $" -- {opponent.name} was defeated!";
                    opponent.hitPoints = 0;

                    attacker.victories++;
                    opponent.defeats++;
                }

                else
                    response.message += $" -- {opponent.name} has {opponent.hitPoints} left!";
                response.data = new AttackResultDTO
                {
                    attacker = attacker.name,
                    attackerHP = attacker.hitPoints,
                    opponent = opponent.name,
                    opponentHP = opponent.hitPoints,
                    damage = damage
                };

                await _dataContext.SaveChangesAsync();
            }

            catch (Exception e)
            {
                response.setErrorMessage(e.Message);   
            }

            return response;
        }

        public async Task<ServiceResponse<AttackResultDTO>> AttackWithSkill(AttackSkillDTO combatInfo)
        {
            var response = new ServiceResponse<AttackResultDTO>();
            try
            {
                var attacker = await _dataContext.characters
                                        .Include(c => c.Skills)
                                        .FirstOrDefaultAsync(c => c.id == combatInfo.attackerID)
                                            ?? throw new Exception("Wrong character ID for attacker.");

                var opponent = await _dataContext.characters
                                        .FirstOrDefaultAsync(c => c.id == combatInfo.opponentID)
                                            ?? throw new Exception("Wrong character ID for the opponent.");

                var attackerSkill = attacker.Skills
                                        .FirstOrDefault(sk => sk.id == combatInfo.skillID)
                                            ?? throw new Exception($"{attacker.name} doesn't know that skill.");


                var rand = new Random();
                //dmg calculation : (weapon + random str) - (opponent defense's buff)
                var damage = GetSkillDamage(attacker, attackerSkill, opponent, rand);
                if (damage < 0) damage = 0;

                response.message = $"{attacker.name} attacks {opponent.name} with {attackerSkill.name}, dealing {damage} damage!";

                opponent.hitPoints -= damage;                
                if (opponent.hitPoints <= 0){
                    response.message += $" -- {opponent.name} was defeated!";
                    opponent.hitPoints = 0;

                    attacker.victories++;
                    opponent.defeats++;
                }

                else
                    response.message += $" -- {opponent.name} has {opponent.hitPoints} left!";
                response.data = new AttackResultDTO {
                    attacker = attacker.name,
                    attackerHP = attacker.hitPoints,
                    opponent = opponent.name,
                    opponentHP = opponent.hitPoints,
                    damage = damage
                };
                
                await _dataContext.SaveChangesAsync();
            }

            catch(Exception e)
            {
                response.setErrorMessage(e.Message);   
            }

            return response;
        }

        public async Task<ServiceResponse<AutomaticFightDTO>> AutomaticFightAll(){
           var array  = await _dataContext.characters.Select(c => c.id).ToArrayAsync();
           return await AutomaticFight(array);
        }


        public async Task<ServiceResponse<AutomaticFightDTO>> AutomaticFight(int[] characterIDs)
        {
            //set up X amount of characters of all users randomly against each other. The fight ends once someone loses, the last one to strike wins.
            var response = new ServiceResponse<AutomaticFightDTO> {
                data = new AutomaticFightDTO()
            };

            try
            {
                Random rand = new Random();
                bool defeated = false;
                var characters = await _dataContext.characters
                    .Include(c => c.Skills)
                    .Include(c => c.weapon)
                    .Where(c => characterIDs.Contains(c.id))
                    .ToListAsync();
                
                List<Character> attackerList;

                //validate given IDs
                if (characters.Count != characterIDs.Length){
                    String incorrectIDs = "";
                    foreach(int id in characterIDs){
                        bool idExists = characters.Any(c => c.id == id);
                        if (!idExists)
                            incorrectIDs +=  ", "+id;
                    }

                    response.setErrorMessage($"The following character IDs do not exist: {incorrectIDs}.");
                }
                
                else
                { //allow the fight to start
                    while(!defeated)
                    {
                        //reseed the attacker list with all characters
                        attackerList = new List<Character>(characters.OrderBy(x => rand.Next()).ToList());

                        while(attackerList.Count > 0){
                            //each attacker only attacks once per loop.
                            var attacker = attackerList[0];
                            attackerList.RemoveAt(0);
                            //get random opponent, excluding the attacker object
                            var opponent = characters
                                            .OrderBy(x => rand.Next())
                                            .First(opp => opp != attacker);
                            
                            //attacker and opponent found, now time to deal damage to opp.
                            //choose method of attack: wep or skill:
                            //furthermore, ensure that the attacker even has any skills.

                            bool choseWeapon = true;
                            if (attacker.Skills.Count > 0)
                                choseWeapon = (rand.Next(2) == 1);
                            string attackMethod = "";
                            int damage = 0;

                            if (choseWeapon){
                                damage = GetWeaponDamage(attacker, opponent, rand);
                                attackMethod = attacker.weapon.name;
                                if (attackMethod.Equals("fists"))
                                    attackMethod = "their fists";
                                defeated = CheckIfDefeated(attacker, opponent, damage);
                            }

                            else //chose Skill
                            {
                                var attackerSkill = attacker.Skills[rand.Next(attacker.Skills.Count)]; //get random skill
                                damage = GetSkillDamage(attacker, attackerSkill, opponent, rand);
                                attackMethod = attackerSkill.name;
                                defeated = CheckIfDefeated(attacker, opponent, damage);
                            }

                            response
                                .data
                                .log
                                .Add($"{attacker.name} attacks {opponent.name} with {attackMethod}, dealing {damage} damage");

                            if (defeated){
                                response.data.log.Add($"{opponent.name} was defeated by {attacker.name}!");
                                break;
                            }
                        }
                    }
                }

                //reset all characters' HP   
                foreach(Character character in characters){
                    character.hitPoints = 100;
                    character.fights++;
                    await _dataContext.SaveChangesAsync();
                }
            }
            catch(Exception e){
                response.setErrorMessage(e.Message);
            }
            return response;
        }

        public async Task<ServiceResponse<List<LeaderboardDTO>>> GetLeaderboard(){
            //get all characters
            //sort them by victories
            //send out leaderboardDTO

            var response = new ServiceResponse<List<LeaderboardDTO>>();
            var charactersList =  _dataContext.characters.ToList();

            response.data = new List<LeaderboardDTO>();
            
            foreach(Character c in charactersList){
                response.data.Add(new LeaderboardDTO 
                    { 
                        name = c.name,
                        victories = c.victories,
                        defeats = c.defeats,
                        fights = c.fights
                    });
            }

        response.data = response.data.OrderByDescending(c => c.victories).ToList();
        return response;
        }

        /*
            HELPER METHODS
        */

        private int GetWeaponDamage(Character attacker, Character opponent, Random rand)
        {
            int damage = (attacker.weapon.damage + rand.Next(attacker.intelligence)) - rand.Next(opponent.defense / 4, opponent.defense);
            return GetDamage(damage);
        }

        private int GetSkillDamage(Character attacker, Skill attackerSkill, Character opponent, Random rand)
        {
            int damage = (attackerSkill.damage + rand.Next(attacker.intelligence)) - rand.Next(opponent.defense / 4, opponent.defense);
            return GetDamage(damage);
        }

        private int GetDamage(int damage) => (damage > 0) ? damage : 0;
        private bool CheckIfDefeated(Character attacker, Character opponent, int damage){
            if(damage > opponent.hitPoints){
                attacker.victories++;
                opponent.defeats++;
                opponent.hitPoints = 0;
            }
            else
                opponent.hitPoints -= damage;
            
            return (opponent.hitPoints == 0);
        }

    }
}