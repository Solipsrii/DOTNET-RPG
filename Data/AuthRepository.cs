
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace DOTNET_RPG.Data
{
    /*
        "repository", being a class that's dedicated to DB access.
    */
    public class AuthRepository : IAuthRepository
    {
        //DEPENDENCY INJECTION of DataContext.
        private DataContext _context;
        //DEPENDENCY INJECTION of the appsettings.json file
        private IConfiguration _configuration;
        public AuthRepository(DataContext context, IConfiguration configuration)
        {
         _context = context;   
         _configuration = configuration;
        }

        public async Task<ServiceResponse<string>> login(string username, string password)
        {
            var response = new ServiceResponse<string>();
            
            var user = await _context.users.FirstOrDefaultAsync(u => u.username.Equals(username));
            if(user == null)
                response.setErrorMessage("No user in database."); //this may be a potential security risk, as attackers can see this message.
            
            //assuming the user was found, verify if the password is correct.
            else if (!verifyPassword(password, user.passwordHash, user.passwordSalt))
                response.setErrorMessage("Username '"+username+"' does not match with given password.");
            
            //assuming the user was found, AND the password is correct, we send the user's ID back via the responseService:
            else {
                response.data = createToken(user);
            }

            return response;
        }

        public async Task<ServiceResponse<int>> register(User user, string password)
        {
            var response = new ServiceResponse<int>();
            user.username = user.username.ToLower();

            if(await userExists(user.username))
                response.setErrorMessage( "User ID already taken.");

            else
            {
            //local declartion of passhash and passsalt via out.
            createPasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.passwordHash = passwordHash;
            user.passwordSalt = passwordSalt;

            //add user to database, and save changes.
            _context.users.Add(user);
            await _context.SaveChangesAsync();
            response.data = user.id;
            }

            return response;
        }

        public async Task<bool> userExists(string username)
        {
            if(await _context.users.AnyAsync(u => u.username.Equals(username)))
                return true;
            return false;
        }
        

    /*
    ############################################################################################################################################
        ## HELPER METHODS ##  ## HELPER METHODS ##  ## HELPER METHODS ##  ## HELPER METHODS ##  ## HELPER METHODS ##  ## HELPER METHODS ##  
    ############################################################################################################################################
    */

        private void createPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt){
            using(var hmac = new System.Security.Cryptography.HMACSHA512()){
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool verifyPassword(string password, byte[] passwordHash, byte[] passwordSalt){
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)){
                byte[] contestedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                
                return passwordHash.SequenceEqual(contestedHash); //checks byte-by-byte if both are equal.
            }
        }
        private string createToken(User user){
            /*
            This is a big one, but in short -- 
            a JWT, json web token, is a string that's saved in a json file (like appsettings.json)
            in a secure way. Rather than saving your password in-session in plain-text, we can 
            encrypt it, save it to file, and decrypt it on-server.

            This requires: 
            json-claims, 
            security-key,
            token descriptor, the handler.
            security token.

            * A "claim" is the identifier of the user (level of security access, name of the user, etc)
            * "security-key" is the private key generated used to decrypt the specific token.
            * "token descriptor" is an object that contains special characteristics of the token, like
                                when should it expire and such.
            * the "handler" serializes the string into bytes, and validates em'. useful shit.
            * security token -- the final garbled string nonsense.
            */

            //claim types-name (id), claim type name.
            List<Claim> claimList = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()), //claim type ID
                new Claim(ClaimTypes.Name, user.username) //Name!
            };

            //fetching the security-token that we save from appsettings.json
            SymmetricSecurityKey? key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            if (key == null)
                throw new Exception("No existing Token!");

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            
            //one last object to prep, contains the qualities needed to create the final token.
            //like when the token should expire, and so on. 
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claimList),  //claims
                Expires =  DateTime.Now.AddDays(1),       //claims
                SigningCredentials = creds              //security key
            };
            //last prep
            //JWT (json web token) token handler. Package provides support to serializing and validating json cred tokens!
            JwtSecurityTokenHandler tokenHandler  = new JwtSecurityTokenHandler();
            //create the token!
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}