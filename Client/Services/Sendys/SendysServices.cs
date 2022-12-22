using BlazorAuth.Shared.Dtos;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorAuth.Client.Services.Sendys;
public class SendysServices : ISendysServices
{
    private readonly HttpClient _httpClient;

    public SendysServices(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }

    public async Task<bool> SaveStrings(IEnumerable<StringValueDto> stringValueDtos)
    {
        try
        {
            var result = await _httpClient.PostAsJsonAsync("sendys/savestrings", stringValueDtos);
            if (!result.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<UserDetailDto> GetUserDetails(string userEmail)
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<UserDetailDto>($"sendys/getuserdetails/{userEmail}");
            return result;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<IEnumerable<StringValueDto>> GetAllStringValues(string userEmail)
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<StringValueDto>>($"sendys/getallstringvalues/{userEmail}");
            return result!;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<string> GetUserId(string userEmail)
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<UserDetailDto>($"sendys/getuserid/{userEmail}");
            return result.UserId;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    // Admin
    public async Task<List<UserDetailDto>> GetAllUsersDetails()
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<UserDetailDto>>($"sendys/getallusers");
            return result;
        }
        catch (Exception ex)
        {
            return null;
        }
    }


}
