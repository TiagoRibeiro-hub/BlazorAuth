using BlazorAuth.Shared.Dtos;

namespace BlazorAuth.Client.Services.Sendys;

public interface ISendysServices
{
    Task<bool> SaveStrings(IEnumerable<StringValueDto> stringValueDtos);
    Task<UserDetailDto> GetUserDetails(string userEmail);
    Task<IEnumerable<StringValueDto>> GetAllStringValues(string userEmail);
    Task<string> GetUserId(string userEmail);

    // Admin
    Task<List<UserDetailDto>> GetAllUsersDetails();
}