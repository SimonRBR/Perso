using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmolPass.Domain.Entities;


namespace SmolPass.Infrastructure.Persistence.Configurations
{
    public class VaultItemConfiguration : IEntityTypeConfiguration<VaultItem>
    {
        public void Configure(EntityTypeBuilder<VaultItem> builder)
        {
            builder.ToTable("VaultItems");
            builder.HasKey(v => v.Id);
            builder.Property(v => v.UserId).IsRequired();
            builder.HasIndex(v => v.UserId);
            builder.HasOne<User>().WithMany().HasForeignKey(v => v.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(v => v.EncryptedBlob).IsRequired().HasColumnType("VARBINARY(MAX)");
            builder.Property(v => v.CreatedAt).IsRequired();
            builder.Property(v => v.UpdatedAt).IsRequired();
        }
    }
}
