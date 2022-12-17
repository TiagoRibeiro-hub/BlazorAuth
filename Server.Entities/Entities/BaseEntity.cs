namespace Server.Entities.Entities;

public class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;

}
