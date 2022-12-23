using BlazorAuth.Client.Services.Sendys;
using BlazorAuth.Shared;
using BlazorAuth.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;

namespace BlazorAuth.Client.Pages;

[Authorize(nameof(Policies.Admin))]
public partial class AllStrings : ComponentBase
{
    [Inject]
    private ISendysServices ISendysServices { get; set; }
    [Inject]
    private NavigationManager NavManager { get; set; }


    public string Email { get; set; } = "";
    public string Message { get; set; } = "";

    private List<UserDetailDto> UserList = new();

    private List<StringValueDto> ArrayStr = new();

    private bool Show { get; set; } = false;

    protected override async Task OnInitializedAsync()
    {
        var result = await ISendysServices.GetAllUsersDetails();
        if (result != null)
        {
            UserList = result;
        }
    }

    public Task OnChange(object value)
    {
        var propValue = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;
        if (propValue != null)
        {
            Email = propValue.ToString()!;
        }
        else
        {
            //
        }
        return Task.CompletedTask;
    }

    public async Task HandleValidSubmit()
    {
        IEnumerable<StringValueDto> result = null;
        if (string.IsNullOrWhiteSpace(Email))
        {
            Message = "Select one User";
        }
        else
        {
            result = await ISendysServices.GetAllStringValues(Email);
            if (result != null)
            {
                ArrayStr = result.ToList();
                Show = true;
            }
        }
    }

    public void BackToSendys()
    {
        NavManager.NavigateTo("sendys");
    }
}

