using System.Security.Claims;
using Messenger.Data_Transfer_Objects.Auth;
using Messenger.Data_Transfer_Objects.Controller_Response_Dto_s;
using Messenger.Data_Transfer_Objects.UserDto_s;
using Messenger.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers;
[ApiController]
[Route("/user")]
public class UserController :ControllerBase
{
    private IUserService _UserRepo;

    public UserController(IUserService userrepo)
    {
        _UserRepo = userrepo;
    }
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult> RegisterUser([FromBody]RegisterDto dto)
    {
        var result = await _UserRepo.RegisterUser(dto);

        if (result.IsSucces) return Ok(result);

        return BadRequest(result);

    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login([FromBody]LoginDto dto)
    {
        var result = await _UserRepo.Login(dto);
        if (result.isSucces) return Ok(result);

        return BadRequest(result);
        





    }


    [AllowAnonymous]
    [HttpPost("refreshtoken")]
    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
    {
      var result = await  _UserRepo.RefreshToken(dto);
      if (result.isSucces) return Ok(result);

      return BadRequest(result);





    }



    
    [HttpPost("uploadprofilephoto")]
    public async Task<ActionResult> UploadProfilePhoto([FromForm] IFormFile Image)
    {
        var currentUserid = GetCurrentUserId();
        if (currentUserid == Guid.Empty) return BadRequest();
        
        var result = await _UserRepo.UploadProfilePhoto(Image, currentUserid);
        if (result.IsSucces) return Ok(result);

        return BadRequest(result);




    }




    [NonAction]
    private Guid GetCurrentUserId()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Guid.Empty;
            Guid.TryParse(userIdClaim.Value, out Guid userId);
            return userId;

        }
        catch (Exception ex)
        {
            return Guid.Empty;
        }
        
        
    }


}