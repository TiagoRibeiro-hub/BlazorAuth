using BlazorAuth.Shared.Enums;
using System.ComponentModel.DataAnnotations;
#nullable disable
namespace Server.Core.PageModels.Account;

public sealed class RegisterUserDetailInputModel
{
    [Required(AllowEmptyStrings = false)]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
    public string FirstName { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
    public string Surname { get; set; }


    [Required]
    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }

    [Required]
    public Gender Gender { get; set; }

}