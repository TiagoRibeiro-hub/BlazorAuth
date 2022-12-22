using BlazorAuth.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace BlazorAuth.Shared.Dtos;
#nullable disable
public sealed class UserDetailDto : BaseDto
{
    public UserDetailDto()
    {

    }

    public UserDetailDto(string userId)
    {
        UserId = userId;
    }

    public string FullName { get { return $"{FirstName} {Surname}"; } }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string Surname { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }

    [Required]
    public Gender? Gender { get; set; }

    public string UserId { get; set; }

    public string Email { get; set; }
}

public class StringValueDto : BaseDto
{
    public string Value { get; set; }

    public string UserId { get; set; }

    public string UserName { get; set; }
}
