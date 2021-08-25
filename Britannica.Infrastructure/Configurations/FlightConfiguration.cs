using Britannica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Britannica.Infrastructure.Configurations
{
    internal class FlightConfiguration : IEntityTypeConfiguration<FlightEntity>
    {
        public void Configure(EntityTypeBuilder<FlightEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(t => t.Origin).HasMaxLength(255).IsRequired();
            builder.Property(t => t.Destination).HasMaxLength(255).IsRequired();
            
            builder
                .HasOne(b => b.Aircraft)
                .WithOne(b => b.Flight)
                .HasForeignKey<AircraftEntity>(b => b.FlightRef);
        }
    }
}
