using Server.Entities.Entities;

namespace Server.Core.Model;

public sealed class ResponseModel
{
    public EmailConfimationTokenModel EmailConfimationToken { get; set; }
    public Microsoft.AspNetCore.Identity.IdentityResult IdentityResult { get; set; }
    public ApplicationUser User { get; set; }    
}


