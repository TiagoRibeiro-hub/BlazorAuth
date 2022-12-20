using Microsoft.AspNetCore.Identity;
using Server.Core.Services.Seed;
using Server.Data;
using Server.Data.Repositories;
using Server.Entities.Entities;

namespace BlazorAuth.Server.Extensions;

public static class WebApplicationExtensions
{
    public static async Task RegisterRoles(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var seed = scope.ServiceProvider.GetRequiredService<ISeed>();
        await seed.CreateRolesAndAdmin();
    }
}
