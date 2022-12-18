

using BlazorAuth.Shared.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.WebUtilities;
using Server.Core.PageModels.Account;
using Server.Entities.Entities;
using System.Text.Encodings.Web;
using System.Text;
using System;
using Microsoft.Extensions.Logging;
using Server.Core.Services.Email;
using Server.Core.Model;

namespace Server.Core.Services;

public class RegisterService : IRegisterService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IUserEmailStore<ApplicationUser> _emailStore;
    private readonly IEmailSender _emailSender;

    public RegisterService(
        UserManager<ApplicationUser> userManager, 
        IUserStore<ApplicationUser> userStore, 
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _userStore = userStore; 
        _emailSender = emailSender;
        _emailStore = _emailSender.GetEmailStore();
    }



    public async Task<ResponseModel> CreateUser(RegisterInputModel input, UserDetailDto userDetailDto)
    {
        var user = ApplicationUserModel.CreateUser();

        user.Detail = ApplicationUserModel.GetUserDetails(userDetailDto);

        await _userStore.SetUserNameAsync(user, input.Email, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, input.Email, CancellationToken.None);
        var result = await _userManager.CreateAsync(user, input.Password);
        
        var responseModel = new ResponseModel()
        {
            IdentityResult = result,
        };
        if (result.Succeeded)
        {

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            responseModel.UrlPageModel = new UrlPageModel
            (
                url: "/Account/ConfirmEmail",
                area: "Identity",
                pageHandler: null,
                userId: userId,
                code: code
            );

        }

        responseModel.User = user;
        return responseModel;
    }

}