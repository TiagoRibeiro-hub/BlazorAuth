// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Core.PageModels.Account;
using BlazorAuth.Shared.Dtos;
using Server.Core.Services.Email;
using Server.Core.Services.Manager;
using Server.Core.Services;
using Microsoft.AspNetCore.Identity;
using Server.Entities.Entities;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Server.Core.Model;

namespace BlazorAuth.Server.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {

        private readonly IManager _manager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IAuthenticationManager _authenticationManager;

        public RegisterModel(
            IManager manager,
            IUserStore<ApplicationUser> userStore,
            ILogger<RegisterModel> logger,
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
        public RegisterInputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [BindProperty]
        public UserDetailDto UserDetail { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _manager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _manager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = ApplicationUserModel.CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                var result = await _authenticationManager.CreateUser(user, Input, UserDetail);

                if (result.IdentityResult.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = result.EmailConfimationToken.UserId, code = result.EmailConfimationToken.Code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_manager.RequireConfirmedAccount())
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _manager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.IdentityResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }


        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_manager.SupportsUserEmail())
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }

    }
}
