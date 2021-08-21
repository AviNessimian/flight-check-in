using Britannica.Application.Bases;
using Britannica.Application.Contracts;
using Britannica.Application.Exceptions;
using Britannica.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Britannica.Application.Interactors
{

    public class GetPassengersRequest : PaginatedRequest, IRequest<List<PassengerEntity>>
    {
        public GetPassengersRequest(int pageIndex, int totalPages)
        {
            PageIndex = pageIndex;
            TotalPages = totalPages;
        }
    }

    internal class GetPassengersInteractor : IRequestHandler<GetPassengersRequest, List<PassengerEntity>>
    {
        private readonly IPassengerRepository _passengerRepository;

        public GetPassengersInteractor(IPassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public async Task<List<PassengerEntity>> Handle(GetPassengersRequest request, CancellationToken cancellationToken)
        {
            var flight = await _passengerRepository.GetPassengers(request.PageIndex, request.TotalPages, cancellationToken);
            _ = flight ?? throw new NotFoundException($"Passengers Not Found");
            return flight;
        }
    }
}
