using Microsoft.AspNetCore.Authorization;

namespace BlazorAuth.Shared;

public static class Policies
{
    // This policy is internal and required for the extension method.
    // You CANNOT use this policy in your controllers or pages.
    internal static void PolicyConfigurationFailedFallback(AuthorizationPolicyBuilder builder) => builder.RequireAssertion(context => false);

    public static void Sendys(AuthorizationPolicyBuilder builder) => builder.RequireAuthenticatedUser().Requirements.Add(new SendysAuthorize());

    public static void Admin(AuthorizationPolicyBuilder builder) => builder.RequireAuthenticatedUser().Requirements.Add(new AdminAuthorize());

    public static void User(AuthorizationPolicyBuilder builder) => builder.RequireAuthenticatedUser().Requirements.Add(new UserAuthorize());
}
