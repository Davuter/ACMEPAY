using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Core.Application.CQRS.Command.Void
{
    public class VoidCommand: IRequest<VoidResponse>
    {
        public Guid Id { get; set; }

        public string OrderReference { get; set; }
    }
}
