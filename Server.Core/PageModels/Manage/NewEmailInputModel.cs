using System.ComponentModel.DataAnnotations;
#nullable disable
namespace Server.Core.PageModels;

public sealed class NewEmailInputModel
{

    [Required]
    [EmailAddress]
    [Display(Name = "New email")]
    public string NewEmail { get; set; }
}