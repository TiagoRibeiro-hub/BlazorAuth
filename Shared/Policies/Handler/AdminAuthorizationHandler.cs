using Microsoft.AspNetCore.Authorization;

namespace BlazorAuth.Shared;

public sealed class AdminAuthorizationHandler : AuthorizationHandler<AdminAuthorize>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminAuthorize requirement)
    {
        if (context.User.IsInRole(Role.Admin))
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}

public sealed class AdminAuthorize : IAuthorizationRequirement
{
}
