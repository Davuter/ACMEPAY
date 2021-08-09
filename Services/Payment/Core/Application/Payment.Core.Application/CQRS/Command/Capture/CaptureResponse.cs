using Payment.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Core.Application.CQRS.Command.Capture
{
    public class CaptureResponse
    {
        public Guid Id { get; set; }

        public TransactionStatus Status { get; set; }
    }
}
