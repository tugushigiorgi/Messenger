using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Messenger.Database.Models;
using Messenger.Services;
using Microsoft.IdentityModel.Tokens;

namespace Messenger.Repository;

public class JwtRepository :IjwtService
{
    private IConfiguration _configuration;

    public JwtRepository(IConfiguration config)
    {
        _configuration = config;

    }



    public string CreateToken(string Email,String UserId)
    {
        var Expiration = DateTime.Now.AddMinutes(Double.Parse(_configuration["Jwt:EXPIRATION_MINUTES"]!));
        Claim[] claims =
        {
            new Claim(JwtRegisteredClaimNames.Email,Email),
             new Claim(JwtRegisteredClaimNames.Sub,UserId)
        };
        var  securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!) );

        var  signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = Expiration,
            SigningCredentials = signingCredentials,
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],  
        };
        var  tokenHandler = new JwtSecurityTokenHandler();
        var createtoken = tokenHandler.CreateToken(descriptor);
        string token =tokenHandler.WriteToken(createtoken);

        return token;




    }





    public bool ValidateExpiredToken(string token)
    {
        var  securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!) );

         var validatedescriptor = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidAudience = _configuration["Jwt:Audience"],
            IssuerSigningKey=securityKey 
        };
        
        
        
        
          var handelr= new JwtSecurityTokenHandler();
          try
          {
              handelr.ValidateToken(token, validatedescriptor, out SecurityToken outtoken);

              return outtoken != null;
          }
          catch (Exception e)
          {
              return false;
          }

             



    }





}