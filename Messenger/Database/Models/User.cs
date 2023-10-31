using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Messenger.Database.Models;

public class User:IdentityUser<Guid>
{
    [Required]
    public string Surname { get; set; }
    
    
}