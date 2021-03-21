using Anazon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Anazon.Infra.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.Property(x => x.Name).HasColumnType("VARCHAR(64)").IsRequired();
            builder.Property(x => x.CPF).HasColumnType("VARCHAR(11)").IsRequired();
            builder.Property(x => x.RG).HasColumnType("VARCHAR(9)").IsRequired(false);
            builder.Property(x => x.Birthdate).IsRequired();
            builder.Property(x => x.Sex).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();

            builder.HasIndex(x => x.CPF).IsUnique();
        }
    }
}
