using BlazorAuth.Client.Services.Sendys;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Net.Http.Headers;

namespace BlazorAuth.Client.Extensions;
public static class ServicesExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ISendysServices, SendysServices>();

        services.AddRadzenServices();

        return services;
    }    
    
    public static IServiceCollection AddRadzenServices(this IServiceCollection services)
    {
        services.AddScoped<DialogService>();
        services.AddScoped<NotificationService>();
        services.AddScoped<TooltipService>();
        services.AddScoped<ContextMenuService>();
        return services;
    }
}

