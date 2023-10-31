using System.ComponentModel.DataAnnotations;

namespace Messenger.Data_Transfer_Objects.UserDto_s;

public class RegisterDto
{
    [EmailAddress]
    [Required]
    public string Email { get; set; }
   
    [Required]
    public string UserName { get; set; }
    
    [Required]
    public string Surname { get; set; }
    
    
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; }
    
    

}