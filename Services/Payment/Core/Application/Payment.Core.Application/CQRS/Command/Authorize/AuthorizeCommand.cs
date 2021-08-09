using MediatR;
using Payment.Core.Domain.Enums;
using System;

namespace Payment.Core.Application.CQRS.Command.Authorize
{
    public class AuthorizeCommand : IRequest<AuthorizeResponse>
    {
        public string OrderReferenceNumber { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CardHolderName { get; set; }
        public string CardPan { get; set; }
        public int CardExpirationMonth { get; set; }
        public int CardExpirationYear { get; set; }
        public int CardCvv { get; set; }
    }
}
