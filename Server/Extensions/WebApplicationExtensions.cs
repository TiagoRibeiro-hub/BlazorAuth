using Server.Core.Services.Seed;


namespace BlazorAuth.Server.Extensions;

public static class WebApplicationExtensions
{
    public static async Task RegisterRoles(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var seed = scope.ServiceProvider.GetRequiredService<ISeed>();

        await seed.CreateRolesAndAdmin();
        await seed.CreateUser();

    }
}
