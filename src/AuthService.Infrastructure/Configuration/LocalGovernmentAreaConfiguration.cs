using AuthService.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Configuration
{
    public class LocalGovernmentAreaConfiguration : IEntityTypeConfiguration<LocalGovernmentArea>
    {
        public void Configure(EntityTypeBuilder<LocalGovernmentArea> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.StateId).IsRequired();
        }
    }
}