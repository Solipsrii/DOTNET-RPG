using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_RPG.DTOs.User
{
    //both userLgoin and userRegister are identical, but still we seperate them, for future-proofing (we may add more shit to the login / register process).
        public class UserLoginDTO
    {
        public string username { get; set; } = "";
        public string password { get; set; } = "";
    }
}