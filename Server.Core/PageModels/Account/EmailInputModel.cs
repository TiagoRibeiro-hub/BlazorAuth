using System.ComponentModel.DataAnnotations;
#nullable disable
namespace Server.Core.PageModels.Account;

public sealed class EmailInputModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
