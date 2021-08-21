﻿using Britannica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Britannica.Infrastructure.Configurations
{
    internal class PassengerFlightSeatConfiguration : IEntityTypeConfiguration<PassengerFlightSeatEntity>
    {
        public void Configure(EntityTypeBuilder<PassengerFlightSeatEntity> builder)
        {
            builder.HasKey(bc => new { bc.FlightId, bc.PassengerId });
        }
    }


}