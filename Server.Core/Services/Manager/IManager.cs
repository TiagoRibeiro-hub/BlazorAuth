using BlazorAuth.Shared.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Server.Core.Model;
using Server.Entities.Entities;

namespace Server.Core.Services.Manager;

public interface IManager
{
    Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync();
    Task<Microsoft.AspNetCore.Identity.IdentityResult> CreateUser(ApplicationUser user, string password);
    Task<EmailConfimationTokenModel> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
    bool RequireConfirmedAccount();
    Task SignInAsync(ApplicationUser user, bool isPersistent, string? authenticationMethod = null);
    Task<ApplicationUser> FindByEmailAsync(string email);
    Task<ApplicationUser> FindByIdAsync(string userId);
    Task<Microsoft.AspNetCore.Identity.IdentityResult> ConfirmEmailAsync(ApplicationUser user, string code);
    Task<SignInResult> PasswordSignInAsync(string email, string password, bool rememberMe, bool lockoutOnFailure);
    Task<Microsoft.AspNetCore.Identity.IdentityResult> ChangeEmailAsync(ApplicationUser user, string email, string code);
    Task<Microsoft.AspNetCore.Identity.IdentityResult> SetUserNameAsync(ApplicationUser user, string email);
    Task RefreshSignInAsync(ApplicationUser user);
    AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);
    Task<ExternalLoginInfo> GetExternalLoginInfoAsync();
    Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor);
    Task<Microsoft.AspNetCore.Identity.IdentityResult> CreateAsync(ApplicationUser user);
    Task<IdentityResult> AddLoginAsync(ApplicationUser user, UserLoginInfo info);
    Task<bool> IsEmailConfirmedAsync(ApplicationUser user);
    Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
    Task<ApplicationUser> GetTwoFactorAuthenticationUserAsync();
    Task<SignInResult> TwoFactorAuthenticatorSignInAsync(string authenticatorCode, bool rememberMe, bool rememberClient);
    Task<string> GetUserIdAsync(ApplicationUser user);
    Task<SignInResult> TwoFactorRecoveryCodeSignInAsync(string recoveryCode);
    Task SignOutAsync();
    Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string code, string password);

    bool SupportsUserEmail();
}
