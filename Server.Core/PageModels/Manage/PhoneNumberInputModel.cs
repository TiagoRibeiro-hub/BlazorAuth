using System.ComponentModel.DataAnnotations;
#nullable disable
namespace Server.Core.PageModels;

public sealed class PhoneNumberInputModel
{
    [Phone]
    [Display(Name = "Phone number")]
    public string PhoneNumber { get; set; }
}
