using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_RPG.Data
{
    public interface IAuthRepository
    {

        //returns string, as a JSON token.
        Task<ServiceResponse<string>> login (string username, string password);
        //returns user ID number
        Task<ServiceResponse<int>> register (User user, string password);
        Task<bool> userExists(string username);
    }
}