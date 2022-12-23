using BlazorAuth.Client;
using BlazorAuth.Client.Auth.CustomUserFactory;
using BlazorAuth.Client.Extensions;
using BlazorAuth.Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("BlazorAuth.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();


// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("BlazorAuth.ServerAPI"));


builder.Services.AddAuthorizationHandlerServices();
builder.Services.AddAuthorizationCore(options => options.AddSharedPolicies());

builder.Services.AddApiAuthorization();
builder.Services.AddApiAuthorization().AddAccountClaimsPrincipalFactory<AccountClaimsPrincipalUserFactory>();

builder.Services.AddServices();

await builder.Build().RunAsync();
