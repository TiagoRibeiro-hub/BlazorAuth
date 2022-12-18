using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Server.Core.Model;

public sealed class EmailConfimationTokenModel
{
    public EmailConfimationTokenModel(string userId, string code)
    {
        UserId = userId;
        Code = code;
    }

    public string UserId { get; set; }
    public string Code { get; set; }
}



