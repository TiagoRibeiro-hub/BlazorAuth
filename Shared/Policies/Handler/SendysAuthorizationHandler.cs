using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace BlazorAuth.Shared;

public sealed class SendysAuthorizationHandler : AuthorizationHandler<SendysAuthorize>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SendysAuthorize requirement)
    {      
        if (context.User.IsInRole(Role.Sendys))
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}

public sealed class SendysAuthorize : IAuthorizationRequirement
{
}



