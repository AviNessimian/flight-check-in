using Britannica.Application.Bases;
using Britannica.Application.Contracts;
using Britannica.Domain.Entities;
using Britannica.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Britannica.Infrastructure.Repositories
{
    internal class FlightRepository : IFlightRepository
    {
        private ApplicationDbContext _applicationDb;

        public FlightRepository(ApplicationDbContext applicationDb)
        {
            _applicationDb = applicationDb;
        }

        public async Task<FlightEntity> Get(int id, CancellationToken cancellationToken)
        {
            var flight = await _applicationDb
                .Flights
                .AsNoTracking()
                .Include(x => x.Aircraft)
                .Include(x => x.PassengerFlights)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            flight.Aircraft.Seats = await GetSeats(flight.Aircraft.Id, cancellationToken);
            return flight;
        }

        public async Task<PaginatedResponse<FlightEntity>> Get(
            int pageIndex,
            int totalPages,
            CancellationToken cancellationToken)
        {
            var flightsQuery = _applicationDb
                .Flights
                .AsNoTracking()
                .Include(x => x.Aircraft)
                .Include(x => x.PassengerFlights);

            var paginatedFlights = await flightsQuery.PaginateAsync(pageIndex, totalPages, cancellationToken);
            foreach (var flight in paginatedFlights)
            {
                flight.Aircraft.Seats = await GetSeats(flight.Aircraft.Id, cancellationToken);
            }
            return paginatedFlights;
        }

        private Task<List<SeatEntity>> GetSeats(int aircraftId, CancellationToken cancellationToken)
        {
            return _applicationDb
                   .Seats
                   .AsNoTracking()
                   .Where(x => x.AircraftRef == aircraftId)
                   .ToListAsync(cancellationToken);
        }
    }
}
