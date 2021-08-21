using Britannica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Britannica.Infrastructure.Configurations
{
    internal class PassengerFlightsEntityConfiguration : IEntityTypeConfiguration<PassengerFlightEntity>
    {
        public void Configure(EntityTypeBuilder<PassengerFlightEntity> builder)
        {
            builder.HasKey(bc => new { bc.FlightId, bc.PassengerId });

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
                .HasForeignKey(bc => new { bc.FlightId, bc.PassengerId });

            builder
             .HasOne(b => b.PassengerFlightSeat)
             .WithOne(b => b.PassengerFlight)
             .HasForeignKey<PassengerFlightSeatEntity>(bc => new { bc.FlightId, bc.PassengerId });
        }
    }


}
