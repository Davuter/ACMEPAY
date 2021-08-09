using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment.Core.Application.CQRS.Command.Authorize;
using Payment.Core.Application.CQRS.Command.Capture;
using Payment.Core.Application.CQRS.Command.Void;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Controllers
{

    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorizeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("api/[controller]")]
        [HttpPost]
        public async Task<AuthorizeResponse> Authorize([FromBody] AuthorizeCommand command)
        {
            return await _mediator.Send(command);
        }

        [Route("api/[controller]/{id}/capture")]
        [HttpPost]
        public async Task<CaptureResponse> Capture(Guid id, [FromBody] string orderReference)
        {
            var command = new CaptureCommand
            {
                Id =id,
                OrderReference = orderReference
            };
            return await _mediator.Send(command);
        }

        [Route("api/[controller]/{id}/voids")]
        [HttpPost]
        public async Task<VoidResponse> Void(Guid id, [FromBody]string orderReference)
        {
            var command = new VoidCommand
            {
                Id =id,
                OrderReference = orderReference
            };
            return await _mediator.Send(command);
        }

    }
}
