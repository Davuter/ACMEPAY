using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payment.Core.Application.CQRS.Query.GetTransactionsByFilter;
using System;
using System.Threading.Tasks;

namespace PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<GetTransactionsQueryResponse> GetTransactions(int pageSize, int pageIndex, DateTime? from, DateTime? to, int status)
        {
            var query = new GetTransactionsQuery
            {
                From = from,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Status = status,
                To = to
            };
          return  await _mediator.Send(query);
        }
    }
}
