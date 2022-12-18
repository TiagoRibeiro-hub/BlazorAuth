// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Server.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Server.Core.Services.Manager;

namespace BlazorAuth.Server.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly IManager _manager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(IManager manager, ILogger<LogoutModel> logger)
        {
            _manager = manager;
            _logger = logger;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _manager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}
