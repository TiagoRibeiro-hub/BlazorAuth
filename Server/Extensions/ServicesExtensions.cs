using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Server.Core.Services;
using Server.Core.Services.Email;
using Server.Core.Services.Manager;
using Server.Data;
using Server.Data.Repositories;
using Server.Entities.Entities;
using Server.Entities.Options;

namespace BlazorAuth.Server.Extensions;
public static class ServicesExtensions
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("ConnectionString").Get<ConnectionStringOptions>();
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString.Default));
    }

    public static IdentityBuilder AddIdentity(this IServiceCollection services)
    {
        return services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;

            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZãáàéêíóõúçÃÁÀÉÊÍÓÕÚÇ1234567890._-!@";
            options.User.RequireUniqueEmail = true;

            options.SignIn.RequireConfirmedAccount = true;
            options.SignIn.RequireConfirmedEmail = true;
            options.SignIn.RequireConfirmedPhoneNumber = false;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
            options.Lockout.MaxFailedAccessAttempts = 5;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
    }

    public static AuthenticationBuilder AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection("Authentication").Get<AuthOptions>();
        return services.AddAuthentication()
            .AddIdentityServerJwt()
            .AddGoogleAuthentication(options.Google);
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IManager, Manager>();
        services.AddScoped<IAuthenticationManager, AuthenticationManager>();
        services.AddScoped<IUserDetailService, UserDetailService>();
    }    
    
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBaseRepository, BaseRepository>();
    }

    public static void AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailOptions>(configuration.GetSection("EmailOptions"));
        services.AddTransient<IEmailSender, EmailSender>();
    }
}
