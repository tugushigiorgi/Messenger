using Messenger.Database;
using Messenger.Database.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Messenger.Repository;

public class CustomUserManager :UserManager<User>
{
    private Dbcontext _dbcontext;
    public CustomUserManager(Dbcontext db,    IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger) 
        : base (store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)

    {
        _dbcontext = db;
    }



    public User? GetUserByRefreshToken(string refreshtoken)
    {
        var getuser = _dbcontext.Users.SingleOrDefault(usr => usr.RefreshToken == refreshtoken);

        return getuser;





    }
    
    
    
    
    
}