using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Core.Application.CQRS.Command.Void
{
    public class VoidCommandValidator : AbstractValidator<VoidCommand>
    {
        public VoidCommandValidator()
        {
            RuleFor(r => r.Id).NotEmpty().NotNull().WithMessage("Invalid Payment Id");
            RuleFor(r => r.OrderReference).NotEmpty().NotNull().MaximumLength(50).WithMessage("Invalid Order Reference");
        }
    }
}
