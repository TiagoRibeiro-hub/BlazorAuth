using BlazorAuth.Shared.Dtos;
using Server.Data.Repositories;
using Server.Entities.Entities;
using Server.Entities.Mapper;

namespace Server.Core.Services;

public sealed class UserDetailService : IUserDetailService
{
    private readonly IBaseRepository _baseRepository;

    public UserDetailService(IBaseRepository baseRepository)
    {
        _baseRepository = baseRepository;
    }

    public async Task<bool> CreateUserDetail(string userId, UserDetailDto userDetailDto) => await _baseRepository.AddWithCheck<UserDetail>(userDetailDto.MapToEntity(userId));
    
    public async Task<bool> HasUserDetail(string userId) => await _baseRepository.Exists<UserDetail>(details => details.UserId == userId);
}
