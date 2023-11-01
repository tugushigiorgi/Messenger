using Messenger.Database.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Database;

public class Dbcontext :IdentityDbContext<User,UserRole,Guid>
{
    public DbSet<ProfilePhoto> ProfilePhoto { get; set; }
    public Dbcontext(DbContextOptions options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>().HasOne(c => c.Profilephoto).WithOne(c => c.user).HasForeignKey<ProfilePhoto>(c=>c.UserId);
        
        base.OnModelCreating(builder);
    }
    
}