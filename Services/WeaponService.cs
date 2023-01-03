using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DOTNET_RPG.DTOs.Character;
using DOTNET_RPG.DTOs.WeaponDTO;

namespace DOTNET_RPG.Services
{
    public class WeaponService : IWeaponService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public WeaponService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
    
        }


         public async Task<ServiceResponse<List<GetWeaponDTO>>> GetWeaponList (){
            //get all list of weapons from _context
            var response = new ServiceResponse<List<GetWeaponDTO>>();
            var weaponList = await _context.weapons.ToListAsync();
            response.data =  _mapper.Map<List<GetWeaponDTO>>(weaponList);

            return response;
         }

        public async Task<ServiceResponse<List<GetWeaponDTO>>> AddWeapon(AddWeaponDTO addWeapon)
        {
            /**
            add a weapon to the DB by first ensuring its name is unique, and then push through a list of all existing weapons.
            */
            var response = new ServiceResponse<List<GetWeaponDTO>>();
            addWeapon.name = addWeapon.name.Trim();

            if(!(await _context.weapons.AnyAsync(w => w.name.ToLower() == addWeapon.name.ToLower())))
            {
                _context.weapons.Add(_mapper.Map<Weapon>(addWeapon));
                await _context.SaveChangesAsync();
                
                response.data = await _context.weapons
                                .Select(w => _mapper.Map<GetWeaponDTO>(w))
                                .ToListAsync();
            }
            else
                response.setErrorMessage("Tried to add an already existing weapon to DB.");

            return response;
        }

        private int GetUserID() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}