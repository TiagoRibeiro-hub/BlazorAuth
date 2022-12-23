using BlazorAuth.Client.PageModel;
using BlazorAuth.Client.Services.Sendys;
using BlazorAuth.Shared;
using BlazorAuth.Shared.Dtos;
using BlazorAuth.Shared.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorAuth.Client.Pages;

[Authorize(nameof(Policies.User))]
public partial class GetStrings : ComponentBase
{
    [Inject]
    private ISendysServices ISendysServices { get; set; }
    [Inject]
    private NavigationManager NavManager { get; set; }


    [CascadingParameter]
    public Task<AuthenticationState> AuthTask { get; set; }

    private System.Security.Claims.ClaimsPrincipal user;

    private NumberModel numberModel = new();
    private StringValueDto[] ArrayStr { get; set; }
    private bool Show { get; set; } = false;
    private bool IsSaved { get; set; } = false;
    private string UserId { get; set; } = "";

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthTask;
        this.user = authState.User;

        UserId = this.user.FindFirst(c => c.Type == "id")?.Value ?? "";
        if (string.IsNullOrEmpty(UserId))
        {
            var userEmail = this.user.FindFirst(c => c.Type == "preferred_username")?.Value ?? string.Empty;
            var userId = await ISendysServices.GetUserId(userEmail);
            if (string.IsNullOrEmpty(userId))
            {
                //
            }
            UserId = userId;
        }
    }

    async Task HandleValidSubmit()
    {
        await SetArrayStr(numberModel);
        if (ArrayStr.Any())
        {
            Show = true;
            IsSaved = false;
        }
    }

    async Task SaveStrings()
    {
        if (ArrayStr.Any())
        {
            IsSaved = await ISendysServices.SaveStrings(ArrayStr);
        }
    }

    void BackToSendys()
    {
        NavManager.NavigateTo("sendys");
    }

    void SeeAll()
    {
        NavManager.NavigateTo("details");
    }

    private Task SetArrayStr(NumberModel inputModel)
    {
        Random rd = new Random();
        ArrayStr = new StringValueDto[inputModel.Number];
        for (int i = 0; i < inputModel.Number; i++)
        {
            string str = Utils.GetStrings(rd);
            ArrayStr[i] = new StringValueDto() { UserId = UserId, Value = str };
        }
        return Task.CompletedTask;
    }
}

