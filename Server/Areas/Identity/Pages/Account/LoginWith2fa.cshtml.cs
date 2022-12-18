// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Server.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Server.Core.PageModels.Account;
using Server.Core.Services.Manager;

namespace BlazorAuth.Server.Areas.Identity.Pages.Account
{
    public class LoginWith2faModel : PageModel
    {
        private readonly IManager _manager;
        private readonly ILogger<LoginWith2faModel> _logger;

        public LoginWith2faModel(IManager manager, ILogger<LoginWith2faModel> logger)
        {
            _manager = manager;
            _logger = logger;
        }

        [BindProperty]
        public TwoFactorAuthInputModel Input { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _manager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            ReturnUrl = returnUrl;
            RememberMe = rememberMe;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            var user = await _manager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            var result = await _manager.TwoFactorAuthenticatorSignInAsync(Input.TwoFactorCode, rememberMe, Input.RememberMachine);

            var userId = await _manager.GetUserIdAsync(user);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", userId);
                return LocalRedirect(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID '{UserId}' account locked out.", userId);
                return RedirectToPage("./Lockout");
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", userId);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return Page();
            }
        }
    }
}
