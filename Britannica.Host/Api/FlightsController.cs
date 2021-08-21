using Britannica.Application.Interactors;
using Britannica.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Britannica.Host.Api
{
    public class FlightsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public FlightsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<FlightEntity>), StatusCodes.Status200OK)]
        public async Task<ActionResult> Get(
            [FromQuery, Required] int pageIndex,
            [FromQuery, Required] int totalPages, 
            CancellationToken cancellationToken)
            => Ok(await _mediator.Send(new GetFlightsRequest(pageIndex, totalPages), cancellationToken));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FlightEntity), StatusCodes.Status200OK)]
        public async Task<ActionResult> Get(
            [FromRoute, Required]int id,
            CancellationToken cancellationToken) 
            => Ok(await _mediator.Send(new GetFlightRequest(id), cancellationToken));


        [HttpPost(nameof(CheckIn))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> CheckIn(
            [FromBody] CheckInRequest request, 
            CancellationToken cancellationToken) 
            => Ok(await _mediator.Send(request, cancellationToken));
    }
}
