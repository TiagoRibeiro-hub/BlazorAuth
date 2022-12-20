using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Server.Entities.Entities;

namespace Server.Data.Configuration
{
    public sealed class StringValueConfig : IEntityTypeConfiguration<StringValue>
    {
        public void Configure(EntityTypeBuilder<StringValue> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnType("UniqueIdentifier");
            builder.Property(x => x.Value).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(x => x.IsActive).HasColumnType("bit").IsRequired();
            builder.Property(x => x.IsDeleted).HasColumnType("bit").IsRequired();
            
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.Value });

            builder.ToTable("StringValues");

            builder.HasOne(x => x.User).WithMany(x => x.StringValues).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
