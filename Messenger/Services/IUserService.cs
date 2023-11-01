using Messenger.Data_Transfer_Objects.Auth;
using Messenger.Data_Transfer_Objects.Controller_Response_Dto_s;
using Messenger.Data_Transfer_Objects.UserDto_s;

namespace Messenger.Services;

public interface IUserService
{

    public Task<ControllerResponse> RegisterUser(RegisterDto dto);

    public Task<LoginResponseDto> Login(LoginDto dto);

    public Task<LoginResponseDto> RefreshToken(RefreshTokenDto dto);

    
    public Task<ControllerResponse> UploadProfilePhoto(IFormFile file,Guid CurrentUserId);
    


}