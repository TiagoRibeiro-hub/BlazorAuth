// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Server.Core.Services.Manager;
using Server.Entities.Entities;

namespace BlazorAuth.Server.Areas.Identity.Pages.Account
{
    public class ConfirmEmailChangeModel : PageModel
    {
        private readonly IManager _manager;

        public ConfirmEmailChangeModel(IManager manager)
        {
            _manager = manager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string email, string code)
        {
            if (userId == null || email == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _manager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _manager.ChangeEmailAsync(user, email, code);
            if (!result.Succeeded)
            {
                StatusMessage = "Error changing email.";
                return Page();
            }

            // In our UI email and user name are one and the same, so when we update the email
            // we need to update the user name.
            var setUserNameResult = await _manager.SetUserNameAsync(user, email);
            if (!setUserNameResult.Succeeded)
            {
                StatusMessage = "Error changing user name.";
                return Page();
            }

            await _manager.RefreshSignInAsync(user);
            StatusMessage = "Thank you for confirming your email change.";
            return Page();
        }
    }
}
