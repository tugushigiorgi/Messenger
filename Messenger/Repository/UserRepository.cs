using Messenger.Data_Transfer_Objects.Controller_Response_Dto_s;
using Messenger.Data_Transfer_Objects.UserDto_s;
using Messenger.Database.Models;
using Messenger.Services;

namespace Messenger.Repository;

public class UserRepository:IUserService
{
    private CustomUserManager _userManager;

    public UserRepository(CustomUserManager manager)
    {
        _userManager = manager;
    }


    public async  Task<ControllerResponse> RegisterUser(RegisterDto dto)
    {
        var checkUserWithEmail =await  _userManager.FindByEmailAsync(dto.Email);
        if (checkUserWithEmail != null)
        {
            return new ControllerResponse
            {
                IsSucces = false,
                Message = "Email is already in use, use different email address"
                
            };


        }

        var newUser = new User { Email = dto.Email,UserName = dto.UserName,Surname = dto.Surname};

         var result =await    _userManager.CreateAsync(newUser, dto.Password);
    
     
     if (result.Succeeded)
     {
         return new ControllerResponse
         {
             IsSucces = true,
             Message = "User Registered Succesfully"

         };
     }



     var Errors = result.Errors;
     var errorsData = "";
     foreach (var error in Errors)
     {
         errorsData += error.Description;
     }
     
     
     return new ControllerResponse
     {
         IsSucces = false,
         Message = "Something Went Wrong :"+errorsData

     };



    }
}