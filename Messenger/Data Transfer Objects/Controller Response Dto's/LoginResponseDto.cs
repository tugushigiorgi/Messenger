namespace Messenger.Data_Transfer_Objects.Controller_Response_Dto_s;

public class LoginResponseDto
{
   public bool isSucces { get; set; }
   public string message { get; set; }
   public string Token { get; set; }
   public string RefreshToken { get; set; }
   
}