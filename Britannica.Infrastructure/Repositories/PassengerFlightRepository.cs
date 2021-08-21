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
            try
            {
                var seat = await _applicationDb.Seats.SingleAsync(x => x.Id == newPassengerFlight.PassengerFlightSeat.SeatId);
                if (!(seat.IsAvailable ?? true))
                {
                    throw new BusinessRuleException($"Seat {seat.Id} is not available");
                }

                seat.IsAvailable = false;
                _applicationDb.PassengerFlights.Add(newPassengerFlight);
                return await _applicationDb.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var exceptionEntry = ex.Entries.Single();
                var databaseEntry = exceptionEntry.GetDatabaseValues();
                if (databaseEntry == null)
                {
                    throw new BusinessRuleException($"Unable to save changes. The Seat was deleted by another user.");
                }
                else
                {
                    /*
                     "The record you attempted to edit "
                            + "was modified by another user after you got the original value. The "
                            + "edit operation was canceled and the current values in the database "
                            + "have been displayed. If you still want to edit this record, click "
                            + "the Save button again. Otherwise click the Back to List hyperlink."
                     */
                }

                return false;
            }
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
