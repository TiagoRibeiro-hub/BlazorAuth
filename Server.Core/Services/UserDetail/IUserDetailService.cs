using BlazorAuth.Shared.Dtos;

namespace Server.Core.Services;
public interface IUserDetailService
{
    Task<bool> CreateUserDetail(string userId, UserDetailDto userDetailDto);
    Task<UserDetailDto> FindByUserId(string userId);
    Task<bool> HasUserDetail(string userId);
}

