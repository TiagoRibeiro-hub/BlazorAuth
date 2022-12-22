using Microsoft.AspNetCore.Authorization;
using Server.Entities.Constants;

namespace BlazorAuth.Server.Handler;

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



