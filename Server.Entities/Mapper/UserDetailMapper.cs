using BlazorAuth.Shared.Dtos;
using Server.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities.Mapper;
public static class UserDetailMapper
{
    public static UserDetail MapToEntity(this UserDetailDto userDetailDto, string userId)
    {
        return new UserDetail()
        {
            FirstName = userDetailDto.FirstName,
            Surname = userDetailDto.Surname,
            BirthDate = userDetailDto.BirthDate,
            Gender = userDetailDto.Gender,
            UserId = userId,
        };
    }
}

