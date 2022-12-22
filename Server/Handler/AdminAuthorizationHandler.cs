﻿using Microsoft.AspNetCore.Authorization;
using Server.Entities.Constants;

namespace BlazorAuth.Server.Handler;

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
