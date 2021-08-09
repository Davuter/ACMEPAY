using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Core.Application.CQRS.Command.Capture
{
    public class CaptureCommand : IRequest<CaptureResponse>
    {
        public Guid Id { get; set; }
        public string OrderReference { get; set; }
    }
}
