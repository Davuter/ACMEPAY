using Payment.Core.Domain.Enums;
using System;


namespace Payment.Core.Application.CQRS.Command.Void
{
    public class VoidResponse
    {
        public Guid Id { get; set; }

        public TransactionStatus Status { get; set; }
    }
}
