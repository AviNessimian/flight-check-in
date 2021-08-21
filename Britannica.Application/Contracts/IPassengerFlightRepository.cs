using Britannica.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Britannica.Application.Contracts
{
    public interface IPassengerFlightRepository
    {
        Task<bool> CheckIn(PassengerFlightEntity newPassengerFlight, CancellationToken cancellationToken);
        Task<PassengerFlightEntity> Get(int flightId, int passengerId, CancellationToken cancellationToken);
    }
}
