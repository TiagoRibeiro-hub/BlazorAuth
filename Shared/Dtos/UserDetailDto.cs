using BlazorAuth.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace BlazorAuth.Shared.Dtos;
#nullable disable
public sealed class UserDetailDto : BaseDto
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string Surname { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; }

    [Required]
    public Gender Gender { get; set; }
}