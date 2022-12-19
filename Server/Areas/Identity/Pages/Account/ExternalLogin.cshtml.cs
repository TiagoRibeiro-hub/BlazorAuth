// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Core.PageModels.Account;
using Server.Core.Services.Email;
using Server.Core.Services.Manager;
using Server.Core.Services;
using Microsoft.AspNetCore.Identity;
using Server.Entities.Entities;
using Server.Core.Model;
using static Duende.IdentityServer.Models.IdentityResources;
using BlazorAuth.Server.Extensions;
using Microsoft.AspNetCore.Authentication;
using BlazorAuth.Shared.Dtos;

namespace BlazorAuth.Server.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly IManager _manager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<ExternalLoginModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IAuthenticationManager _authenticationManager;

        public ExternalLoginModel(
            IManager manager,
            IUserStore<ApplicationUser> userStore,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender,
            IAuthenticationManager authenticationManager)
        {
            _manager = manager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
            _emailSender = emailSender;
            _authenticationManager = authenticationManager;
        }



        [BindProperty]
        public EmailInputModel Input { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [TempData]
        public string Details { get; set; }

        public IActionResult OnGet() => RedirectToPage("./Login");

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _manager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var info = await _manager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _manager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                var email = info.GetClaim(ClaimTypes.Email);

                if (!string.IsNullOrEmpty(email))
                {
                    var hasDetails = await _authenticationManager.HasUserDetail(email);
                    if (!hasDetails)
                    {                    
                        var givenName = info.GetClaim(ClaimTypes.GivenName) ?? "";
                        var surname = info.GetClaim(ClaimTypes.Surname) ?? "";
                        Details = string.Join("/", givenName , surname);
                        return RedirectToPage($"/Account/UserDetail");
                    }
                }
                var userDetails = await _authenticationManager.FindByUserEmail(email);
                if (userDetails != null)
                {
                    User.AddUserDetailClaim(userDetails);        
                    await _manager.AddClaimAsync(email, userDetails.GetClaims());
                    var props = new AuthenticationProperties();
                    props.StoreTokens(info.AuthenticationTokens);
                    props.IsPersistent = false;
                }
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                var email = info.GetClaim(ClaimTypes.Email);
                if (!string.IsNullOrEmpty(email))
                {
                    Input = new EmailInputModel
                    {
                        Email = email
                    };
                }
                return Page();
            }
        }


        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _manager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                var user = ApplicationUserModel.CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                var result = await _authenticationManager.ExternalLogin(user, info);

                if (result.IdentityResult.Succeeded)
                {
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = result.EmailConfimationToken.UserId, code = result.EmailConfimationToken.Code },
                        protocol: Request.Scheme);


                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (_manager.RequireConfirmedAccount())
                        {
                            return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
                        }

                        await _manager.SignInAsync(result.User, isPersistent: false, info.LoginProvider);
                        return LocalRedirect(returnUrl);

                }
                foreach (var error in result.IdentityResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            return Page();
        }

        public IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_manager.SupportsUserEmail())
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
