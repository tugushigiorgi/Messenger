using Messenger.Database;
using Messenger.Database.Models;
using Messenger.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Dbcontext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddIdentity<User, UserRole>(
        options =>
        {
            options.Password.RequireUppercase = false;
            options.Password.RequireDigit = false;
 
        }

    ).AddEntityFrameworkStores<Dbcontext>()
    .AddRoleStore<RoleStore<UserRole, Dbcontext, Guid>>()
    .AddUserStore<UserStore<User, UserRole, Dbcontext, Guid>>()
    .AddUserManager<CustomUserManager>()
    .AddDefaultTokenProviders();







var app = builder.Build();








app.Run();