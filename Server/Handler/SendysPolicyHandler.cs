using Microsoft.AspNetCore.Authorization;
using Server.Entities.Constants;

namespace BlazorAuth.Server.Handler;

public sealed class SendysPolicyHandler : AuthorizationHandler<SendysPolicyHandler>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SendysPolicyHandler requirement)
    {
        if (context.User.IsInRole(Role.AdminSendys) || context.User.IsInRole(Role.UserSendys))
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}

