using BlazorAuth.Shared.Enums;
using System.Reflection;

namespace Server.Entities.Entities;
#nullable disable
public sealed class UserDetail : BaseEntity
{
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}
