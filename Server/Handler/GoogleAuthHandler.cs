using Google.Apis.Auth.OAuth2;
using Google.Apis.PeopleService.v1;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication;
using BlazorAuth.Server.Extensions;
using System.Security.Claims;

namespace BlazorAuth.Server.Handler
{
    public class GoogleAuthHandler
    {
        public Task HandleOnCreatingTicket(OAuthCreatingTicketContext context)
        {
            var credentials = GoogleCredential.FromAccessToken(context.AccessToken);
            var peopleService = new PeopleServiceService(new BaseClientService.Initializer { HttpClientInitializer = credentials });

            var personRequest = peopleService.People.Get("people/me");
            personRequest.OauthToken = context.AccessToken;
            personRequest.PersonFields = "birthdays,genders,locales,locations,clientData,userDefined";

            var person = personRequest.Execute();
            var claimsIdentity = context.Principal?.Identities.First();

            claimsIdentity?.AddGoogleConfigClaims(person);

            var tokens = context.Properties.GetTokens().ToList();

            tokens.Add(new AuthenticationToken()
            {
                Name = "TicketCreated",
                Value = DateTime.UtcNow.ToString()
            });

            context.Properties.StoreTokens(tokens);

            return Task.CompletedTask;
        }
    }
}
