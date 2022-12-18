using BlazorAuth.Shared.Dtos;
using Server.Core.PageModels.Account;
using Server.Core.Model;
using Server.Core.Services.Manager;
using Microsoft.AspNetCore.Identity;


namespace Server.Core.Services;

public class AuthenticationManager : IAuthenticationManager
{
    private readonly IManager _manager;

    public AuthenticationManager(IManager manager)
    {
        _manager = manager;

    }

    public async Task<ResponseModel> CreateUser(RegisterInputModel input, UserDetailDto userDetailDto)
    {
        var user = await _manager.CreateUser(userDetailDto);
        await _manager.SetUserStore(user, input.Email, CancellationToken.None);
        var result = await _manager.CreateUser(user, input.Password);
        
        var responseModel = new ResponseModel()
        {
            IdentityResult = result,
        };
        if (result.Succeeded)
        {
            responseModel.EmailConfimationToken = await _manager.GenerateEmailConfirmationTokenAsync(user);
            responseModel.User = user;
        }
        return responseModel;
    }

    public async Task<ResponseModel> ExternalLogin(string email, UserLoginInfo info)
    {
        var user = ApplicationUserModel.CreateUser();
        await _manager.SetUserStore(user, email, CancellationToken.None);

        var result = await _manager.CreateAsync(user);
        var responseModel = new ResponseModel()
        {
            IdentityResult = result,
        };

        if (result.Succeeded)
        {
            result = await _manager.AddLoginAsync(user, info);
            if (result.Succeeded)
            {
                var callBack = await _manager.GenerateEmailConfirmationTokenAsync(user);
            }
            responseModel.User = user;
        }
        return responseModel;
    }
}