
namespace DOTNET_RPG.Data
{
    /*
        "repository", being a class that's dedicated to DB access.
    */
    public class AuthRepository : IAuthRepository
    {
        //DEPENDENCY INJECTION of DataContext.
        DataContext _context;
        public AuthRepository(DataContext context)
        {
         _context = context;   
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
                response.data = user.id.ToString();
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
    }
}