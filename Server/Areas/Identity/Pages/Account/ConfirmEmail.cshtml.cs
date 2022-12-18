// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Server.Core.Services.Manager;

namespace BlazorAuth.Server.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly IManager _manager;

        public ConfirmEmailModel(IManager manager)
        {
            _manager = manager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _manager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _manager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            return Page();
        }
    }
}
