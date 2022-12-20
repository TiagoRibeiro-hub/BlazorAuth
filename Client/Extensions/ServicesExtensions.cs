using BlazorAuth.Client.Services.Sendys;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorAuth.Client.Extensions;
public static class ServicesExtensions
{
    public static IHttpClientBuilder AddSendysServices(this IServiceCollection services, string baseAddress)
    {
        return services.AddHttpClient<ISendysServices, SendysServices>(client =>
        {
            client.BaseAddress = new Uri(baseAddress);
        });
    }
}

