using BlazorAuth.Shared.Enums;
using Server.Entities.Constants;
using System.ComponentModel.DataAnnotations;
#nullable disable
namespace Server.Core.PageModels.Account;

public sealed class RegisterUserDetailInputModel
{
    [Required(AllowEmptyStrings = false)]
    [RegularExpression(RegularExpressions.NameRegex, ErrorMessage = "Name can only contain letters")]
    public string FirstName { get; set; }

    [Required(AllowEmptyStrings = false)]
    [RegularExpression(RegularExpressions.NameRegex, ErrorMessage = "Surname can only contain letters")]
    public string Surname { get; set; }


    [Required]
    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }

    [Required]
    public Gender Gender { get; set; }

}