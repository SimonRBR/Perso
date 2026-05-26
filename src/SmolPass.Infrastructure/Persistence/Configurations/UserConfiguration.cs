using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmolPass.Domain.Entities;


namespace SmolPass.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.AuthSalt).IsRequired().HasColumnType("VARBINARY(32)");
            builder.Property(u => u.AuthHash).IsRequired().HasColumnType("VARBINARY(32)");
            builder.Property(u => u.EncryptionSalt).IsRequired().HasColumnType("VARBINARY(32)");
            builder.Property(u => u.KdfIterations).IsRequired();
            builder.Property(u => u.CreatedAt).IsRequired();
        }
    }
}
