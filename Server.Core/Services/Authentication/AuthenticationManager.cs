using BlazorAuth.Shared.Dtos;
using Server.Core.PageModels.Account;
using Server.Core.Model;
using Server.Core.Services.Manager;
using Microsoft.AspNetCore.Identity;
using Server.Entities.Entities;
using Server.Data.Repositories;
using static Duende.IdentityServer.Models.IdentityResources;
using BlazorAuth.Shared;

namespace Server.Core.Services;

public class AuthenticationManager : IAuthenticationManager
{
    private readonly IManager _manager;
    private readonly IUserDetailService _userDetailService;

    public AuthenticationManager(
        IManager manager,
        IUserDetailService userDetailService)
    {
        _manager = manager;
        _userDetailService = userDetailService;
    }

    public async Task<ResponseModel> CreateUser(ApplicationUser user, RegisterInputModel input, UserDetailDto userDetailDto)
    {
        user.Detail = ApplicationUserModel.GetUserDetails(userDetailDto);
        var result = await _manager.CreateUser(user, input.Password);

        var responseModel = new ResponseModel()
        {
            IdentityResult = result,
        };
        if (result.Succeeded)
        {
            responseModel.EmailConfimationToken = await _manager.GenerateEmailConfirmationTokenAsync(user);
            responseModel.User = user;
            await _manager.AddToRolesAsync(user, new[] { Role.Sendys, Role.User });
        }
        return responseModel;
    }

    public async Task<ResponseModel> ExternalLogin(ApplicationUser user, UserLoginInfo info)
    {
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
                responseModel.EmailConfimationToken = await _manager.GenerateEmailConfirmationTokenAsync(user);
            }
            responseModel.User = user;
            await _manager.AddToRolesAsync(user, new[] { Role.Sendys, Role.User });
        }
        return responseModel;
    }

    public async Task<bool> HasUserDetail(string email)
    {
        var user = await _manager.FindByEmailAsync(email);
        return await _userDetailService.HasUserDetail(user.Id);
    }

    public async Task<UserDetailDto> FindByUserEmail(string email)
    {
        var user = await _manager.FindByEmailAsync(email);
        return await _userDetailService.FindByUserId(user.Id);
    }


    public async Task<UserDetailDto> CreateUserDetail(string email, UserDetailDto userDetailDto)
    {
        var user = await _manager.FindByEmailAsync(email);
        if (await _userDetailService.CreateUserDetail(user.Id, userDetailDto))
        {
            userDetailDto.UserId = user.Id;
            return userDetailDto;
        }
        return null;
    }

    public async Task<UserDetailDto> FindByUserId(string userId) => await _userDetailService.FindByUserId(userId);

}