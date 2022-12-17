using System.ComponentModel.DataAnnotations;
#nullable disable
namespace Server.Core.PageModels.Account;

public sealed class ResetPasswordInputModel : RegisterInputModel
{

    [Required]
    public string Code { get; set; }
}
