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
        private readonly IManager _manager;
        public UserDetailModel(
            IAuthenticationManager authenticationManager, 
            IManager manager)
        {
            _authenticationManager = authenticationManager;
            _manager = manager;
        }

        [BindProperty]
        public UserDetailDto UserDetail { get; set; }
        public string ReturnUrl { get; set; }

        [TempData]
        public string Details { get; set; }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            var division = Details.IndexOf('/');
            UserDetail = new UserDetailDto() 
            { 
                FirstName = Details.Substring(0, division),
                Surname = Details.Substring(division + 1),
                BirthDate = null,
                Gender = null
            };
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var email = claimsIdentity.GetClaim(ClaimTypes.Email).Value;
                var userDetails = await _authenticationManager.CreateUserDetail(email, UserDetail);
                if(userDetails != null)
                {
                    User.AddUserDetailClaim(userDetails);
                    await _manager.AddClaimAsync(email, userDetails.GetClaims());
                }
                return LocalRedirect(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

    }
}
