// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Server.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Server.Core.PageModels.Account;
using Server.Core.Services.Email;
using Server.Core.Services.Manager;
using Server.Core.Model;

namespace BlazorAuth.Server.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly IManager _manager;
        private readonly IEmailSender _emailSender;

        public ResendEmailConfirmationModel(IManager manager, IEmailSender emailSender)
        {
            _manager = manager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public EmailInputModel Input { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _manager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
                return Page();
            }
            var emaiConfirmationToken = await _manager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = emaiConfirmationToken.UserId, code = emaiConfirmationToken.Code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(
                Input.Email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
            return Page();
        }
    }
}
