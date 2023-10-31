using Messenger.Database;
using Messenger.Database.Models;
using Messenger.Repository;
using Messenger.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Repositories
builder.Services.AddScoped<IUserService, UserRepository>();

builder.Services.AddControllers();

builder.Services.AddDbContext<Dbcontext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddIdentity<User, UserRole>(
        options =>
        {
            options.Password.RequireUppercase = false;
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
        }

    ).AddEntityFrameworkStores<Dbcontext>()
    .AddRoleStore<RoleStore<UserRole, Dbcontext, Guid>>()
    .AddUserStore<UserStore<User, UserRole, Dbcontext, Guid>>()
    .AddUserManager<CustomUserManager>()
    .AddDefaultTokenProviders();







var app = builder.Build();




app.MapControllers();



app.Run();