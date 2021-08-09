using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Core.Application.CQRS.Command.Authorize
{
    public class AuthorizeCommandValidator : AbstractValidator<AuthorizeCommand>
    {
        public AuthorizeCommandValidator()
        {
            RuleFor(r => r.Amount).GreaterThan(0).NotNull().WithMessage("Invalid Amount!");
            RuleFor(r => r.CardCvv).NotNull().LessThan(1000).GreaterThan(0).WithMessage("Invalid Card Cvv Number");
            RuleFor(r => r.CardExpirationMonth).NotNull().NotEmpty().LessThan(13).GreaterThan(0).WithMessage("Invalid Card Expiration Month");
            RuleFor(r => r.CardExpirationYear).NotNull().NotEmpty().WithMessage("Invalid Card Expiration Month");
            RuleFor(r => r.OrderReferenceNumber).NotNull().NotEmpty().MaximumLength(50).WithMessage("Invalid Order Reference Number");
            RuleFor(r => r.Currency).NotNull().NotEmpty().WithMessage("Invalid Currency");
            RuleFor(r => r.CardHolderName).NotNull().NotEmpty().NotEmpty().WithMessage("Invalid Card Holder Name");
            RuleFor(r => r.CardPan).NotNull().MaximumLength(16).WithMessage("Invalid Card Number");
        }
    }
}
