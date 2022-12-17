using System.ComponentModel.DataAnnotations;
#nullable disable
namespace Server.Core.PageModels;

public sealed class DeletePersonalDataInputModel
{
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
