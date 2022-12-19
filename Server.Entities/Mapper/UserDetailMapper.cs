using BlazorAuth.Shared.Dtos;
using Server.Entities.Entities;


namespace Server.Entities.Mapper;
public static class UserDetailMapper
{
    public static UserDetail MapToEntity(this UserDetailDto userDetailDto, string userId = "")
    {
        return new UserDetail()
        {
            FirstName = userDetailDto.FirstName,
            Surname = userDetailDto.Surname,
            BirthDate = userDetailDto.BirthDate!.Value,
            Gender = userDetailDto.Gender!.Value,
            UserId = userId,
        };
    }

    public static UserDetailDto MapToDto(this UserDetail userDetail)
    {
        return new UserDetailDto()
        {
            FirstName = userDetail.FirstName,
            Surname = userDetail.Surname,
            BirthDate = userDetail.BirthDate,
            Gender = userDetail.Gender,
            UserId = userDetail.UserId,
        };
    }
}

