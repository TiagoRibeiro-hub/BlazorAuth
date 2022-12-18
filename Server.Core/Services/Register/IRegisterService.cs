﻿

using BlazorAuth.Shared.Dtos;
using Microsoft.AspNetCore.Identity;
using Server.Core.Model;
using Server.Core.PageModels.Account;
using Server.Entities.Entities;

namespace Server.Core.Services;

public interface IRegisterService
{
    Task<ResponseModel> CreateUser(RegisterInputModel input, UserDetailDto userDetailDto);

}
