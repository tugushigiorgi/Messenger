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
    public async Task<ActionResult> RegisterUser(RegisterDto dto)
    {
        var result = await _UserRepo.RegisterUser(dto);

        if (result.IsSucces) return Ok(result);

        return BadRequest(result);

    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login(LoginDto dto)
    {
        var result = await _UserRepo.Login(dto);
        if (result.isSucces) return Ok(result);

        return BadRequest(result);
        





    }


    [HttpPost("test")]
    public string test() => "succcess";




}