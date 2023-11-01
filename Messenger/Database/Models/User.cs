using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Messenger.Database.Models;

public class User:IdentityUser<Guid>
{
    [Required]
    public string Surname { get; set; }
    
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
    
    public ProfilePhoto? Profilephoto { get; set; }
    
    
     
        
     
    
    
}