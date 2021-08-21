using Britannica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Britannica.Infrastructure.Repositories
{
    internal class AircraftRepository : IAircraftRepository
    {
        private ApplicationDbContext _applicationDb;

        public AircraftRepository(ApplicationDbContext applicationDb)
        {
            _applicationDb = applicationDb;
        }

        public Task<AircraftEntity> Get(int aircraftId, CancellationToken cancellationToken)
        {
            return _applicationDb
               .Aircrafts
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.Id == aircraftId, cancellationToken);
        }
    }
}
