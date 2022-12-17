using BlazorAuth.Shared.Enums;

namespace BlazorAuth.Shared.Dtos;
#nullable disable
public sealed class UserDetailDto : BaseDto
{
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
}