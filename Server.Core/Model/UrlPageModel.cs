using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Server.Core.Model;

public sealed class UrlPageModel
{
    public UrlPageModel(string url, string area, string? pageHandler, string userId, string code)
    {
        Url = url;
        Area = area;
        PageHandler = pageHandler;
        UserId = userId;
        Code = code;
    }

    public string Url { get; set; }
    public string Area { get; set; }
    public string? PageHandler { get; set; }
    public string UserId { get; set; }
    public string Code { get; set; }
}


