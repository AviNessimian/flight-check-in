using Britannica.Application.Bases;
using Britannica.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Britannica.Application.Contracts
{
    public interface IPassengerRepository
    {
        Task<PaginatedResponse<PassengerEntity>> GetPassengers(int pageIndex, int totalPages, CancellationToken cancellationToken);
        Task<PassengerEntity> Get(int id, CancellationToken cancellationToken);

        Task<bool> CheckIn(PassengerFlightEntity newPassengerFlight, CancellationToken cancellationToken);
        Task<PassengerFlightEntity> Get(int flightId, int passengerId, CancellationToken cancellationToken);
    }
}
