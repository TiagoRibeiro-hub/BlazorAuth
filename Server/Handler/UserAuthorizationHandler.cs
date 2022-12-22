using Microsoft.AspNetCore.Authorization;
using Server.Entities.Constants;

namespace BlazorAuth.Server.Handler;

public sealed class UserAuthorizationHandler : AuthorizationHandler<UserAuthorize>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserAuthorize requirement)
    {
        if (context.User.IsInRole(Role.User))
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}

public sealed class UserAuthorize : IAuthorizationRequirement
{
}