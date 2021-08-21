using Britannica.Application.Bases;
using Britannica.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Britannica.Application.Contracts
{
    public interface IFlightRepository
    {
        Task<PaginatedResponse<FlightEntity>> Get(int pageIndex, int totalPages, CancellationToken cancellationToken);
        Task<FlightEntity> Get(int id, CancellationToken cancellationToken);
    }
}



