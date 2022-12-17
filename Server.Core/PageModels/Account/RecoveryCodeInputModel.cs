using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
#nullable disable
namespace Server.Core.PageModels.Account;

public sealed class RecoveryCodeInputModel
{
    [BindProperty]
    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "Recovery Code")]
    public string RecoveryCode { get; set; }
}