using BlazorAuth.Shared.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Server.Core.Model;
using Server.Entities.Entities;
using System.Security.Claims;

namespace Server.Core.Services.Manager;

public interface IManager
{
    #region SignInManager
    AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);
    Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor);
    Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync();
    Task<ExternalLoginInfo> GetExternalLoginInfoAsync();
    Task<SignInResult> PasswordSignInAsync(string email, string password, bool rememberMe, bool lockoutOnFailure);
    Task RefreshSignInAsync(ApplicationUser user);
    Task SignInAsync(ApplicationUser user, bool isPersistent, string? authenticationMethod = null);
    Task<ApplicationUser> GetTwoFactorAuthenticationUserAsync();
    Task<SignInResult> TwoFactorAuthenticatorSignInAsync(string authenticatorCode, bool rememberMe, bool rememberClient);
    Task<SignInResult> TwoFactorRecoveryCodeSignInAsync(string recoveryCode);
    Task SignOutAsync();
    #endregion

    #region UserManager
    Task<Microsoft.AspNetCore.Identity.IdentityResult> CreateUser(ApplicationUser user, string password);
    Task<EmailConfimationTokenModel> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
    bool RequireConfirmedAccount();
    Task<ApplicationUser> FindByEmailAsync(string email);
    Task<ApplicationUser> FindByIdAsync(string userId);
    Task<Microsoft.AspNetCore.Identity.IdentityResult> ConfirmEmailAsync(ApplicationUser user, string code);
    Task<Microsoft.AspNetCore.Identity.IdentityResult> ChangeEmailAsync(ApplicationUser user, string email, string code);
    Task<Microsoft.AspNetCore.Identity.IdentityResult> SetUserNameAsync(ApplicationUser user, string email);
    Task<Microsoft.AspNetCore.Identity.IdentityResult> CreateAsync(ApplicationUser user);
    Task<IdentityResult> AddLoginAsync(ApplicationUser user, UserLoginInfo info);
    Task<bool> IsEmailConfirmedAsync(ApplicationUser user);
    Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
    Task<string> GetUserIdAsync(ApplicationUser user);
    Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string code, string password);
    bool SupportsUserEmail();
    Task<IdentityResult> AddClaimByUserEmailAsync(string email, IEnumerable<Claim> claims);
    Task<IdentityResult> AddClaimByUserIdAsync(string userId, IEnumerable<Claim> claims);
    Task<ApplicationUser> GetUserAsync(ClaimsPrincipal user);
    string GetUserId(ClaimsPrincipal user);
    Task<bool> VerifyTwoFactorTokenAsync(ApplicationUser user, string verificationCode);
    Task<IdentityResult> SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled);
    Task<int> CountRecoveryCodesAsync(ApplicationUser user);
    Task<IEnumerable<string>> GenerateNewTwoFactorRecoveryCodesAsync(ApplicationUser user, int number);
    Task<string> GetAuthenticatorKeyAsync(ApplicationUser user);
    Task<IdentityResult> ResetAuthenticatorKeyAsync(ApplicationUser user);
    Task<string> GetEmailAsync(ApplicationUser user);
    Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
    #endregion
}
