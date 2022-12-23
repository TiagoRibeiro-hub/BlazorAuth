using BlazorAuth.Shared.Dtos;
using Microsoft.AspNetCore.Identity;
using Server.Core.Model;
using Server.Core.PageModels.Account;
using Server.Entities.Entities;

namespace Server.Core.Services;

public interface IAuthenticationManager
{
    Task<RegisterResponseModel> CreateUser(ApplicationUser user, RegisterInputModel input, UserDetailDto userDetailDto);
    Task<RegisterResponseModel> ExternalLogin(ApplicationUser user, UserLoginInfo info);
    Task<bool> HasUserDetail(string email);
    Task<UserDetailDto> FindByUserEmail(string email);
    Task<UserDetailDto> CreateUserDetail(string email, UserDetailDto userDetailDto);
    Task<UserDetailDto> FindByUserId(string userId);
}
