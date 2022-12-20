namespace Server.Entities.Entities;

public class StringValue : BaseEntity
{
    public string Value { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}
