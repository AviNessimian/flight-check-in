using Britannica.Application.Bases;
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

        public Task<PassengerEntity> GetPassenger(int id, CancellationToken cancellationToken)
        {
            return _applicationDb
                .Passengers
                .Include(x => x.PassengerFlights)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<PaginatedResponse<PassengerEntity>> GetPassengers(
            int pageIndex,
            int totalPages,
            CancellationToken cancellationToken)
        {
            var passengersQuery = _applicationDb
                .Passengers
                .Include(x => x.PassengerFlights)
                .AsNoTracking();

            return await passengersQuery.PaginateAsync(pageIndex, totalPages, cancellationToken);
        }
    }
}
