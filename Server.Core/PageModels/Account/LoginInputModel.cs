using System.ComponentModel.DataAnnotations;
#nullable disable
namespace Server.Core.PageModels.Account;

public sealed class LoginInputModel
{

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}
