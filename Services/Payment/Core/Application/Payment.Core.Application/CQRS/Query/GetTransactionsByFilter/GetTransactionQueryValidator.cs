using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Core.Application.CQRS.Query.GetTransactionsByFilter
{
    public class GetTransactionQueryValidator : AbstractValidator<GetTransactionsQuery>
    {
        public GetTransactionQueryValidator()
        {
            RuleFor(r => r.PageIndex).GreaterThan(0);
            RuleFor(r => r.PageSize).GreaterThan(0);
        }
    }
}
