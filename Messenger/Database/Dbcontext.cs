using Messenger.Database.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Database;

public class Dbcontext :IdentityDbContext<User,UserRole,Guid>
{

    public Dbcontext(DbContextOptions options) : base(options)
    { }




}