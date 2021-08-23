using Britannica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Britannica.Infrastructure.Configurations
{
    internal class PassengerFlightsEntityConfiguration : IEntityTypeConfiguration<PassengerFlightEntity>
    {
        public void Configure(EntityTypeBuilder<PassengerFlightEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.HasAlternateKey(bc => new { bc.FlightId, bc.PassengerId, bc.SeatId });

            builder.Ignore(c => c.BaggagesCount);

            builder
                .HasOne(bc => bc.Flight)
                .WithMany(b => b.PassengerFlights)
                .HasForeignKey(bc => bc.FlightId);

            builder
                .HasOne(bc => bc.Passenger)
                .WithMany(c => c.PassengerFlights)
                .HasForeignKey(bc => bc.PassengerId);

            builder
                .HasMany(b => b.Baggages)
                .WithOne(b => b.PassengerFlight)
                .HasForeignKey(bc => new { bc.PassengerFlightRef });

            //builder
            // .HasOne(b => b.PassengerFlightSeat)
            // .WithOne(b => b.PassengerFlight)
            // .HasForeignKey<PassengerFlightSeatEntity>(bc => new { bc.FlightIdRef, bc.PassengerIdRef });
        }
    }
}
