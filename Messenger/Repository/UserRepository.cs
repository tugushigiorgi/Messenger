using Messenger.Data_Transfer_Objects.Auth;
using Messenger.Data_Transfer_Objects.Controller_Response_Dto_s;
using Messenger.Data_Transfer_Objects.UserDto_s;
using Messenger.Database.Models;
using Messenger.Services;

namespace Messenger.Repository;

public class UserRepository : IUserService
{
    private CustomUserManager _userManager;
    private IjwtService _jwtService;
    private IConfiguration _configuration;
    public UserRepository(IConfiguration configuration,IjwtService jwtService ,CustomUserManager manager)
    {
        _userManager = manager;
        _jwtService = jwtService;
         _configuration = configuration;
    }


    public async Task<ControllerResponse> RegisterUser(RegisterDto dto)
    {
        var checkUserWithEmail = await _userManager.FindByEmailAsync(dto.Email);
        if (checkUserWithEmail != null)
        {
            return new ControllerResponse
            {
                IsSucces = false,
                Message = "Email is already in use, use different email address"

            };


        }

        var newUser = new User { Email = dto.Email, UserName = dto.UserName, Surname = dto.Surname };

        var result = await _userManager.CreateAsync(newUser, dto.Password);


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
            Message = "Something Went Wrong :" + errorsData

        };



    }

    public async Task<LoginResponseDto> Login(LoginDto dto)
    {
        var getuser = await _userManager.FindByEmailAsync(dto.Email);
        if (getuser == null)
        {
            return new LoginResponseDto
            {
                isSucces = false,
                message = "Email Not Found"

            };
        }

        var resut = await _userManager.CheckPasswordAsync(getuser, dto.Password);

        if (!resut)
        {
            return new LoginResponseDto
            {
                isSucces = false,
                message = "Password is Incorrect"

            };
        }

        var createToken = _jwtService.CreateToken(getuser.Email!, getuser.Id.ToString());

        var refreshToken = Guid.NewGuid().ToString();
        getuser.RefreshToken = refreshToken;
        getuser.RefreshTokenExpiration=DateTime.Now.AddMinutes(Double.Parse(_configuration["Jwt:REFRESH_TOKEN_EXPIRATION_DAYS"]!));


        return new LoginResponseDto
        {
            Token = createToken,
            RefreshToken = refreshToken,
            isSucces = true,
            message = "Succesfully generated Token & Refresh Token"
        };


    }

}