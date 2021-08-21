using Britannica.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

public interface IAircraftRepository
{
    Task<AircraftEntity> Get(int aircraftId, CancellationToken cancellationToken);
    Task<SeatEntity> GetSeat(int seatId, CancellationToken cancellationToken);
}



