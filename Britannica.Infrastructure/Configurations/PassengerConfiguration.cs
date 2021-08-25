using Britannica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Britannica.Infrastructure.Configurations
{
    internal class PassengerConfiguration : IEntityTypeConfiguration<PassengerEntity>
    {
        public void Configure(EntityTypeBuilder<PassengerEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(t => t.FirtName).HasMaxLength(50).IsRequired();
            builder.Property(t => t.LastName).HasMaxLength(50).IsRequired();
        }
    }
}
