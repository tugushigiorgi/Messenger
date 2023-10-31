using Messenger.Data_Transfer_Objects.Controller_Response_Dto_s;
using Messenger.Database.Models;

namespace Messenger.Services;

public interface IjwtService
{

    public string CreateToken(string Email,String UserId);
    
    




}