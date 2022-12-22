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

    public static UserDetailDto MapToDto(this UserDetail userDetail, string email = "")
    {
        return new UserDetailDto()
        {
            FirstName = userDetail.FirstName,
            Surname = userDetail.Surname,
            BirthDate = userDetail.BirthDate,
            Gender = userDetail.Gender,
            UserId = userDetail.UserId,
            Email = email
        };
    }
}

public static class StringValueMapper
{
    public static StringValue MapToEntity(this StringValueDto stringValueDto)
    {
        return new StringValue()
        {
            Value = stringValueDto.Value,
            UserId = stringValueDto.UserId
        };
    }    
    
    public static IEnumerable<StringValue> MapListToEntity(this IEnumerable<StringValueDto> stringValueDto)
    {
        var list = new List<StringValue>();  
        foreach (var item in stringValueDto)
        {
            list.Add(new StringValue()
            {
                Value = item.Value,
                UserId = item.UserId
            });
        }
        return list;
    }

    public static StringValueDto MapToDto(this StringValue stringValue)
    {
        return new StringValueDto()
        {
            Value = stringValue.Value,
            UserId = stringValue.UserId,
        };
    }

    public static IEnumerable<StringValueDto> MapListToDto(this IEnumerable<StringValue> stringValue, string userName = "")
    {
        var list = new List<StringValueDto>();
        foreach (var item in stringValue)
        {
            list.Add(new StringValueDto()
            {
                Value = item.Value,
                UserId = item.UserId,
                UserName = userName
            });
        }
        return list;
    }
}
