using Microsoft.AspNetCore.Authorization;


namespace BlazorAuth.Shared;

public sealed class UserAuthorizationHandler : AuthorizationHandler<UserAuthorize>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserAuthorize requirement)
    {
        if (context.User.IsInRole(Role.Admin) || context.User.IsInRole(Role.User))
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}

public sealed class UserAuthorize : IAuthorizationRequirement
{
}