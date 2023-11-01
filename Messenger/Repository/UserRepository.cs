using Messenger.Data_Transfer_Objects.Auth;
using Messenger.Data_Transfer_Objects.Controller_Response_Dto_s;
using Messenger.Data_Transfer_Objects.UserDto_s;
using Messenger.Database;
using Messenger.Database.Models;
using Messenger.Services;

namespace Messenger.Repository;

public class UserRepository : IUserService
{
    private CustomUserManager _userManager;
    private IjwtService _jwtService;
    private IConfiguration _configuration;
    private Dbcontext _dbcontext;
    public UserRepository(Dbcontext dbcontext,IConfiguration configuration,IjwtService jwtService ,CustomUserManager manager)
    {
        _userManager = manager;
        _jwtService = jwtService;
         _configuration = configuration;
         _dbcontext = dbcontext;
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
        getuser.RefreshTokenExpiration=DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:REFRESH_TOKEN_EXPIRATION_DAYS"]!));
       await  _dbcontext.SaveChangesAsync();

        return new LoginResponseDto
        {
            Token = createToken,
            RefreshToken = refreshToken,
            isSucces = true,
            message = "Succesfully generated Token & Refresh Token"
        };


    }



    public async Task<LoginResponseDto> RefreshToken(RefreshTokenDto dto)
    {
        
        
        var checkTokenValidation = _jwtService.ValidateExpiredToken(dto.AccessToken);
        if (!checkTokenValidation) return new LoginResponseDto { isSucces = false,message = "Acces Token is Invalid"};

        var getuser = _userManager.GetUserByRefreshToken(dto.RefreshToken);
        if (getuser == null) return new LoginResponseDto { isSucces = false, message = "Refresh Token is Invalid" };

        if (getuser.RefreshTokenExpiration >= DateTime.Now)
        {
            var newRefreshToken = Guid.NewGuid().ToString();
            var NewToken = _jwtService.CreateToken(getuser.Email, getuser.Id.ToString());
            getuser.RefreshTokenExpiration =
                DateTime.Now.AddDays(double.Parse(_configuration["Jwt:REFRESH_TOKEN_EXPIRATION_DAYS"]!));

            getuser.RefreshToken = newRefreshToken;

            await _dbcontext.SaveChangesAsync();
            
            
            
            
            return new LoginResponseDto { Token = NewToken,RefreshToken = newRefreshToken,isSucces = true};
            




        }

        return new LoginResponseDto { message = "Refresh token is Expired",isSucces = false};




    }

    public async  Task<ControllerResponse> UploadProfilePhoto(IFormFile file,Guid CurrentUserId)
    {
        var getuser = await _userManager.FindByIdAsync(CurrentUserId.ToString());
        
        
        var uploadLocation = _configuration["ProfilePhotosDir"];
        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        var filePath = Path.Combine(uploadLocation!, uniqueFileName);
        try
        {
            await using(var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
        catch (Exception e)
        {
            return new ControllerResponse
            {
                IsSucces = false, Message = $"Error While Coping file to the Directory "
            };
        }

        var Profilephoto = new ProfilePhoto
        {
            user = getuser!,
            FileName = file.FileName,
            PublicUrl = $"/{uploadLocation}/{uniqueFileName}",
            FileType = file.ContentType,
            FileExtension = Path.GetExtension(file.FileName),
            FileSize = file.Length,
            UploadDateTime = DateTime.UtcNow,
        };

        _dbcontext.ProfilePhoto.Add(Profilephoto);


        try
        {
            await  _dbcontext.SaveChangesAsync();
            
        }
        catch (Exception e)
        {
            return new ControllerResponse
            {
                IsSucces = false, Message = "Error While Saving in Database"
            };
        }
        return new ControllerResponse
        {
            IsSucces = true, Message = "Profile Photo Uploaded Succesfully"
        };

    }
}