using Britannica.Application.Contracts;
using Britannica.Domain.Entities;
using Britannica.Domain.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Britannica.Application.Interactors
{
    /// <exception cref="NotFoundException">Thrown when flight number of the request not Found.</exception>
    public class GetFlightRequest : IRequest<FlightEntity>
    {
        public GetFlightRequest(int id)
        {
            Id = id;
        }
        public int Id { get; }
    }

    internal class GetFlightInteractor : IRequestHandler<GetFlightRequest, FlightEntity>
    {
        private readonly IFlightRepository _flightRepository;

        public GetFlightInteractor(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public async Task<FlightEntity> Handle(GetFlightRequest request, CancellationToken cancellationToken)
        {
            var flight = await _flightRepository.Get(request.Id, cancellationToken);
            _ = flight ?? throw new NotFoundException($"Flight number {request.Id} not Found");
            return flight;
        }
    }
}
