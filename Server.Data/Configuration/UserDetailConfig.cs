using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Server.Entities.Entities;

namespace Server.Data.Configuration
{
    public sealed class UserDetailConfig : IEntityTypeConfiguration<UserDetail>
    {
        public void Configure(EntityTypeBuilder<UserDetail> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnType("UniqueIdentifier");
            builder.Property(x => x.FirstName).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(x => x.Surname).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(x => x.BirthDate).HasColumnType("datetime2").IsRequired();
            builder.Property(x => x.Gender).HasConversion<int>().IsRequired();
            builder.Property(x => x.IsActive).HasColumnType("bit").IsRequired();
            builder.Property(x => x.IsDeleted).HasColumnType("bit").IsRequired();
            
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.FirstName, x.Surname});

            builder.ToTable("User");
        }
    }
}
