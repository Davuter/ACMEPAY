using MediatR;
using Payment.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Core.Application.CQRS.Query.GetTransactionsByFilter
{
    public class GetTransactionsQuery : IRequest<GetTransactionsQueryResponse>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int? Status { get; set; }

    }
}
