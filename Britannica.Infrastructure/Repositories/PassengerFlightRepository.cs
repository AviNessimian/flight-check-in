using Britannica.Application.Contracts;
using Britannica.Application.Exceptions;
using Britannica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Britannica.Infrastructure.Repositories
{
    internal class PassengerFlightRepository : IPassengerFlightRepository
    {
        private ApplicationDbContext _applicationDb;

        public PassengerFlightRepository(ApplicationDbContext applicationDb)
        {
            _applicationDb = applicationDb;
        }

        public async Task<bool> CheckIn(PassengerFlightEntity newPassengerFlight, CancellationToken cancellationToken)
        {
            var seat = await _applicationDb.Seats.SingleAsync(x => x.Id == newPassengerFlight.PassengerFlightSeat.SeatId);
            seat.IsAvailable = false;
            _applicationDb.PassengerFlights.Add(newPassengerFlight);
            return await _applicationDb.SaveChangesAsync(cancellationToken) > 0;
        }

        public Task<PassengerFlightEntity> Get(int flightId, int passengerId, CancellationToken cancellationToken)
        {
            return _applicationDb
                .PassengerFlights
                .Include(x => x.Baggages)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.FlightId == flightId && x.PassengerId == passengerId, cancellationToken);
        }
    }
}
