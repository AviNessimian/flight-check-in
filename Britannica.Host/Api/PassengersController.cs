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
    public class PassengersController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public PassengersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<PassengerEntity>), StatusCodes.Status200OK)]
        public async Task<ActionResult> Get(
            [FromQuery, Required] int pageIndex,
            [FromQuery, Required] int totalPages, 
            CancellationToken cancellationToken)
             => Ok(await _mediator.Send(new GetPassengersRequest(pageIndex, totalPages), cancellationToken));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PassengerEntity), StatusCodes.Status200OK)]
        public async Task<ActionResult> Get(
            [FromRoute, Required] int id,
            CancellationToken cancellationToken)
            => Ok(await _mediator.Send(new GetPassengerRequest(id), cancellationToken));


        [HttpGet("/flights")]
        [ProducesResponseType(typeof(PassengerEntity), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetFlights(
            [FromRoute, Required] int id,
            CancellationToken cancellationToken)
            => Ok(await _mediator.Send(new GetPassengerRequest(id), cancellationToken));
    }
}
