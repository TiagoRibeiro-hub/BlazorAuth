using BlazorAuth.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using BlazorAuth.Client.Services.Sendys;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using BlazorAuth.Shared.Dtos;

namespace BlazorAuth.Client.Pages;

[Authorize(nameof(Policies.User))]
public partial class Details : ComponentBase
{
    [Inject]
    private ISendysServices ISendysServices { get; set; }
    [Inject]
    private NavigationManager NavManager { get; set; }
    [Inject]
    private HttpClient Http { get; set; }

    [CascadingParameter]
    public Task<AuthenticationState> AuthTask { get; set; }

    private ClaimsPrincipal user;

    private UserDetailDto userDetailDto = new UserDetailDto();
    private string birthDate { get; set; } = "";
    private string gender { get; set; } = "";

    private bool Show { get; set; } = false;
    private bool showDetails { get; set; } = false;

    private List<StringValueDto> ArrayStr = new List<StringValueDto>();

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthTask;
        this.user = authState.User;

        if (this.user.Identity.IsAuthenticated)
        {
            userDetailDto.FirstName = this.user.FindFirst(c => c.Type == "given_name")?.Value ?? string.Empty;
            userDetailDto.Surname = this.user.FindFirst(c => c.Type == "family_name")?.Value ?? string.Empty;
            birthDate = this.user.FindFirst(c => c.Type == "birthdate")?.Value ?? string.Empty;
            gender = this.user.FindFirst(c => c.Type == "gender")?.Value ?? string.Empty;

            var userEmail = user.FindFirst(c => c.Type == "preferred_username")?.Value ?? string.Empty;

            if (string.IsNullOrEmpty(userDetailDto.FirstName) || string.IsNullOrEmpty(userDetailDto.Surname) || string.IsNullOrEmpty(birthDate) || string.IsNullOrEmpty(gender))
            {
                var userDetails = await ISendysServices.GetUserDetails(userEmail);
                if (userDetails != null)
                {
                    userDetailDto.FirstName = userDetails.FirstName;
                    userDetailDto.Surname = userDetails.Surname;
                    birthDate = userDetails.BirthDate.Value.Date.ToShortDateString()!;
                    gender = userDetails.Gender.ToString()!;

                    showDetails = true;
                }
            }
            else
            {
                showDetails = true;
            }

            var result = await ISendysServices.GetAllStringValues(userEmail);
            if (result != null)
            {
                ArrayStr = result.ToList();
                if (ArrayStr.Any())
                {
                    Show = true;
                }
            }
        }
    }

    void BackToSendys()
    {
        NavManager.NavigateTo("sendys");
    }

    void GetStrings()
    {
        NavManager.NavigateTo("getstrings");
    }

}

