using Britannica.Application.Bases;
using Britannica.Application.Contracts;
using Britannica.Domain.Exceptions;
using Britannica.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Britannica.Application.Interactors
{
    /// <exception cref="NotFoundException">Thrown when flights not found.</exception>
    public class GetFlightsRequest : PaginatedRequest, IRequest<List<FlightEntity>>
    {
        public GetFlightsRequest(int pageIndex, int totalPages)
        {
            PageIndex = pageIndex;
            TotalPages = totalPages;
        }
    }

    internal class GetEnvironmentsInteractor : IRequestHandler<GetFlightsRequest, List<FlightEntity>>
    {
        private readonly IFlightRepository _flightRepository;

        public GetEnvironmentsInteractor(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public async Task<List<FlightEntity>> Handle(GetFlightsRequest request, CancellationToken cancellationToken)
        {
            var flights = await _flightRepository.Get(request.PageIndex, request.TotalPages, cancellationToken);
            _ = flights ?? throw new NotFoundException($"Flights not found");
            return flights;
        }
    }
}
