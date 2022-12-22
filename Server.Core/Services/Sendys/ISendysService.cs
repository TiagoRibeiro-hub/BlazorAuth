using BlazorAuth.Shared.Dtos;

namespace Server.Core.Services.Sendys;

public interface ISendysService
{
    Task<IEnumerable<StringValueDto>> GetAllStringValues(string userEmail);
    Task<List<UserDetailDto>> GetAllUsers();
    Task<UserDetailDto> GetUserDetails(string userEmail);
    Task<string> GetUserId(string userEmail);
    Task<bool> SaveStrings(IEnumerable<StringValueDto> stringValueDtos);
}

