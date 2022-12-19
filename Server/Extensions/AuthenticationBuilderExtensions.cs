using BlazorAuth.Server.Handler;
using Google.Apis.PeopleService.v1;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Server.Entities.Options;
using System.Security.Claims;


namespace BlazorAuth.Server.Extensions;

public static class AuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddGoogleAuthentication(this AuthenticationBuilder services, ProviderOptions provider)
    {
        return services.AddGoogle(provider.Name, options =>
        {
            options.ClientId = provider.ClientId;
            options.ClientSecret = provider.ClientSecret;

            options.Scope.Add(PeopleServiceService.Scope.UserinfoProfile);
            options.Scope.Add(PeopleServiceService.Scope.UserinfoEmail);
            options.Scope.Add(PeopleServiceService.Scope.UserBirthdayRead);

            options.Scope.Add(PeopleServiceService.Scope.UserGenderRead);
            options.Scope.Add(PeopleServiceService.Scope.UserPhonenumbersRead);

            options.ClaimActions.Clear();
            options.ClaimActions.MapUniqueJsonKey(ClaimTypes.NameIdentifier, JwtClaimTypes.Id, "string");
            options.ClaimActions.MapUniqueJsonKey(ClaimTypes.Name, JwtClaimTypes.Name, "string");
            options.ClaimActions.MapUniqueJsonKey(ClaimTypes.GivenName, JwtClaimTypes.GivenName, "string");
            options.ClaimActions.MapUniqueJsonKey(ClaimTypes.Surname, JwtClaimTypes.FamilyName, "string");
            options.ClaimActions.MapUniqueJsonKey(ClaimTypes.Email, JwtClaimTypes.Email, "string");

            options.AccessType = "offline";
            options.UsePkce = true;
            options.SaveTokens = true;

            var googleHandler = new GoogleAuthHandler();

            options.Events = new OAuthEvents
            {
                OnCreatingTicket = googleHandler.HandleOnCreatingTicket
            };
        });
    }

}
