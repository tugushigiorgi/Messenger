using Messenger.Database;
using Messenger.Database.Models;
using Messenger.Repository;
using Messenger.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Repositories
builder.Services.AddScoped<IUserService, UserRepository>();
builder.Services.AddScoped<IjwtService, JwtRepository>();


builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder("Bearer").RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
    
    
});

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



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;


}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey =
            new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))



    };


});

builder.Services.AddAuthorization();

var app = builder.Build();






app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();