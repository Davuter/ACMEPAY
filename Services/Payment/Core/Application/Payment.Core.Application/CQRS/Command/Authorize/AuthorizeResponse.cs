using Payment.Core.Domain.Enums;
using System;


namespace Payment.Core.Application.CQRS.Command.Authorize
{
    public class AuthorizeResponse
    {
        public Guid Id { get; set; }
        public TransactionStatus Status { get; set; }
    }
}
