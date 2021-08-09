using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Core.Application.CQRS.Command.Capture
{
    public class CaptureCommandValidator : AbstractValidator<CaptureCommand>
    {
        public CaptureCommandValidator()
        {
            RuleFor(r => r.Id).NotEmpty().NotNull().WithMessage("Invalid Payment Id");
            RuleFor(r => r.OrderReference).NotEmpty().NotNull().MaximumLength(50).WithMessage("Invalid Order Reference");
        }
    }
}
