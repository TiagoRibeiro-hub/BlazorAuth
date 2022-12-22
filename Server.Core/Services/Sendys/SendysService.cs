using BlazorAuth.Shared.Dtos;
using Server.Core.Services.Manager;
using Server.Data.Repositories;
using Server.Entities.Entities;
using Server.Entities.Mapper;

namespace Server.Core.Services.Sendys;

public sealed class SendysService : ISendysService
{
    private readonly IBaseRepository _baseRepository;
    private readonly IManager _manager;
    private readonly IUserDetailService _userDetailService;

    public SendysService(
        IBaseRepository baseRepository,
        IManager manager,
        IUserDetailService userDetailService)
    {
        _baseRepository = baseRepository;
        _manager = manager;
        _userDetailService = userDetailService;
    }

    public async Task<IEnumerable<StringValueDto>> GetAllStringValues(string userEmail)
    {
        var user = await _manager.FindByEmailAsync(userEmail);
        if (user != null)
        {
            var list = await _baseRepository.GetAll<StringValue>(w => w.UserId == user.Id.ToString());

            if (list != null && list.Any())
            {
                return list.MapListToDto(userEmail);
            }
        }
        return null;
    }

    public async Task<List<UserDetailDto>> GetAllUsers()
    {
        var users = await _baseRepository.GetAllUsers();
        if (users != null && users.Any())
        {
            return users.Select(x => x.Detail.MapToDto(x.Email)).ToList();
        }
        return null;
    }

    public async Task<UserDetailDto> GetUserDetails(string userEmail)
    {
        var user = await _manager.FindByEmailAsync(userEmail);
        if (user != null)
        {
            var result = await _userDetailService.FindByUserId(user.Id.ToString(), userEmail);
            if (result != null)
            {
                return result;

            }
        }
        return null;
    }

    public async Task<string> GetUserId(string userEmail)
    {
        var user = await _manager.FindByEmailAsync(userEmail);
        if (user != null)
        {
            return user.Id.ToString();
        }
        return null;
    }

    public async Task<bool> SaveStrings(IEnumerable<StringValueDto> stringValueDtos)
    {
        var stringValue = await _baseRepository.AddRange<StringValue>(stringValueDtos.MapListToEntity());
        return stringValue != null;
    }
}

