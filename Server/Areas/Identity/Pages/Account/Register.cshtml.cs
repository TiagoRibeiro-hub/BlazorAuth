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

namespace BlazorAuth.Server.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IManager _manager;
        public RegisterModel(
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IAuthenticationManager authenticationManager,
            IManager manager)
        {
            _logger = logger;
            _emailSender = emailSender;
            _authenticationManager = authenticationManager;
            _manager = manager;
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
                var result = await _authenticationManager.CreateUser(Input, UserDetail); 

                if (result.IdentityResult.Succeeded)
                {
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
                        await _manager.SignInAsync(result.User, isPersistent: false);
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


    }
}
