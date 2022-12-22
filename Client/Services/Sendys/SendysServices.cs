using BlazorAuth.Shared.Dtos;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorAuth.Client.Services.Sendys;
public class SendysServices : ISendysServices
{
    private readonly HttpClient _httpClient;
    private readonly IAccessTokenProvider _accessTokenProvider;

    public SendysServices(
        HttpClient httpClient, 
        IAccessTokenProvider accessTokenProvider)
    {
        _httpClient = httpClient;
        _accessTokenProvider = accessTokenProvider;
    }

    public async Task<bool> SaveStrings(IEnumerable<StringValueDto> stringValueDtos)
    {
        try
        {
            await GetAccessToken();
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
            await GetAccessToken();
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
            await GetAccessToken();
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
            await GetAccessToken();
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
            await GetAccessToken();
            var result = await _httpClient.GetFromJsonAsync<List<UserDetailDto>>($"sendys/getallusers");
            return result;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private async Task GetAccessToken()
    {
        var accessToken = await _accessTokenProvider.RequestAccessToken();
        if (accessToken.TryGetToken(out var token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
        }
    }
}
