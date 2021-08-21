using Britannica.Application.Bases;
using Britannica.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Britannica.Application.Contracts
{
    public interface IPassengerRepository
    {
        Task<PaginatedResponse<PassengerEntity>> GetPassengers(int pageIndex, int totalPages, CancellationToken cancellationToken);
        Task<PassengerEntity> GetPassenger(int id, CancellationToken cancellationToken);
    }
}
