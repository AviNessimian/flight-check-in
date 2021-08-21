using Britannica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Britannica.Infrastructure.Configurations
{
    internal class AircraftConfiguration : IEntityTypeConfiguration<AircraftEntity>
    {
        public void Configure(EntityTypeBuilder<AircraftEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(t => t.SeatsCount).IsRequired();
            builder.Property(t => t.BaggagesLimit).IsRequired();

            builder.Ignore(c => c.SeatsCount);
            builder.Ignore(c => c.PassengerBagsLimit);

            builder
                .HasMany(b => b.Seats)
                .WithOne(b => b.Flight);
        }
    }
}
