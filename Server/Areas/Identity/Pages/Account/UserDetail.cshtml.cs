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
using System.Security.Claims;
using BlazorAuth.Server.Extensions;

namespace BlazorAuth.Server.Areas.Identity.Pages.Account
{
    public class UserDetailModel : PageModel
    {

        private readonly IAuthenticationManager _authenticationManager;

        public UserDetailModel(IAuthenticationManager authenticationManager)
        {
            _authenticationManager = authenticationManager;
        }

        [BindProperty]
        public UserDetailDto UserDetail { get; set; }
        public string ReturnUrl { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var email = claimsIdentity.GetClaim(ClaimTypes.Email).Value;
                _ = _authenticationManager.CreateUserDetail(email, UserDetail).ConfigureAwait(false);
                return LocalRedirect(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

    }
}
