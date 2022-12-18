// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Core.Services.Manager;

namespace BlazorAuth.Server.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly IManager _manager;

        public RegisterConfirmationModel(IManager manager)
        {
            _manager = manager;
 
        }

        public string Email { get; set; }
        public bool DisplayConfirmAccountLink { get; set; }
        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }
            returnUrl = returnUrl ?? Url.Content("~/");

            var user = await _manager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            Email = email;

            if (DisplayConfirmAccountLink)
            {       
                var emailConfimationToken = await _manager.GenerateEmailConfirmationTokenAsync(user);
                EmailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = emailConfimationToken.UserId, code = emailConfimationToken.Code, returnUrl = returnUrl },
                    protocol: Request.Scheme);
            }

            return Page();
        }
    }
}
