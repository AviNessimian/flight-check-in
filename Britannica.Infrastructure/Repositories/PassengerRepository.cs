﻿using Britannica.Application.Bases;
using Britannica.Application.Contracts;
using Britannica.Domain.Entities;
using Britannica.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Britannica.Infrastructure.Repositories
{
    internal class PassengerRepository : IPassengerRepository
    {
        private ApplicationDbContext _applicationDb;

        public PassengerRepository(ApplicationDbContext applicationDb)
        {
            _applicationDb = applicationDb;
        }

        public Task<PassengerEntity> Get(int id, CancellationToken cancellationToken)
        {
            return _applicationDb
                .Passengers
                .AsNoTracking()
                .Include(x => x.PassengerFlights)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<PaginatedResponse<PassengerEntity>> GetPassengers(
            int pageIndex,
            int totalPages,
            CancellationToken cancellationToken)
        {
            var passengersQuery = _applicationDb
                .Passengers
                .AsNoTracking()
                .Include(x => x.PassengerFlights);

            return await passengersQuery.PaginateAsync(pageIndex, totalPages, cancellationToken);
        }

        public async Task<bool> CheckIn(PassengerFlightEntity newPassengerFlight, CancellationToken cancellationToken)
        {
            var seat = await _applicationDb.Seats.SingleAsync(x => x.Id == newPassengerFlight.SeatId);
            seat.IsAvailable = false;
            _applicationDb.PassengerFlights.Add(newPassengerFlight);
            return await _applicationDb.SaveChangesAsync(cancellationToken) > 0;
        }

        public Task<PassengerFlightEntity> GetFlight(int id, CancellationToken cancellationToken)
        {
            return _applicationDb
                .PassengerFlights
                .AsNoTracking()
                .Include(x => x.Baggages)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }

}
