using System.ComponentModel.DataAnnotations;

namespace BlazorAuth.Client.PageModel;
public class NumberModel
{
    [Required]
    [Range(1, 500)]
    public int Number { get; set; }
}

