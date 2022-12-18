using BlazorAuth.Shared.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Server.Core.Model;
using Server.Core.Services.Email;
using Server.Entities.Entities;
using System.Text;
using static IdentityModel.OidcConstants;

namespace Server.Core.Services.Manager;

public sealed class Manager : IManager
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public Manager(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<Microsoft.AspNetCore.Identity.IdentityResult> AddLoginAsync(ApplicationUser user, UserLoginInfo info) => await _userManager.AddLoginAsync(user, info);

    public async Task<Microsoft.AspNetCore.Identity.IdentityResult> ChangeEmailAsync(ApplicationUser user, string email, string code) => await _userManager.ChangeEmailAsync(user, email, code);

    public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl) => _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

    public async Task<Microsoft.AspNetCore.Identity.IdentityResult> ConfirmEmailAsync(ApplicationUser user, string code) => await _userManager.ConfirmEmailAsync(user, code);

    public async Task<Microsoft.AspNetCore.Identity.IdentityResult> CreateAsync(ApplicationUser user) => await _userManager.CreateAsync(user);

    public async Task<Microsoft.AspNetCore.Identity.IdentityResult> CreateUser(ApplicationUser user, string password) => await _userManager.CreateAsync(user, password);

    public Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor) => _signInManager.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent: isPersistent, bypassTwoFactor: bypassTwoFactor);

    public async Task<ApplicationUser> FindByEmailAsync(string email) => await _userManager.FindByEmailAsync(email);

    public async Task<ApplicationUser> FindByIdAsync(string userId) => await _userManager.FindByIdAsync(userId);

    public async Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync() => await _signInManager.GetExternalAuthenticationSchemesAsync();

    public Task<ExternalLoginInfo> GetExternalLoginInfoAsync() => _signInManager.GetExternalLoginInfoAsync();

    public async Task<SignInResult> PasswordSignInAsync(string email, string password, bool rememberMe, bool lockoutOnFailure) => await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: lockoutOnFailure);

    public async Task RefreshSignInAsync(ApplicationUser user) => await _signInManager.RefreshSignInAsync(user);

    public bool RequireConfirmedAccount() => _userManager.Options.SignIn.RequireConfirmedAccount;

    public async Task<EmailConfimationTokenModel> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
    {
        var userId = await GetUserIdAsync(user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        return new EmailConfimationTokenModel
            (
                userId: userId,
                code: code
            );
    }

    public async Task<Microsoft.AspNetCore.Identity.IdentityResult> SetUserNameAsync(ApplicationUser user, string email) => await _userManager.SetUserNameAsync(user, email);

    public async Task SignInAsync(ApplicationUser user, bool isPersistent, string? authenticationMethod = null) => await _signInManager.SignInAsync(user, isPersistent: isPersistent, authenticationMethod: authenticationMethod);

    public async Task<bool> IsEmailConfirmedAsync(ApplicationUser user) => await _userManager.IsEmailConfirmedAsync(user);

    public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
    {
        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
    } 

    public async Task<ApplicationUser> GetTwoFactorAuthenticationUserAsync() => await _signInManager.GetTwoFactorAuthenticationUserAsync();

    public async Task<SignInResult> TwoFactorAuthenticatorSignInAsync(string authenticatorCode, bool rememberMe, bool rememberClient) => await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode.Replace(" ", string.Empty).Replace("-", string.Empty), rememberMe, rememberClient);
 
    public async Task<string> GetUserIdAsync(ApplicationUser user) => await _userManager.GetUserIdAsync(user);

    public async Task<SignInResult> TwoFactorRecoveryCodeSignInAsync(string recoveryCode) => await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode.Replace(" ", string.Empty));

    public async Task SignOutAsync() => await _signInManager.SignOutAsync();

    public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string code, string password) => await _userManager.ResetPasswordAsync(user, code, password);

    public bool SupportsUserEmail() => _userManager.SupportsUserEmail;
}