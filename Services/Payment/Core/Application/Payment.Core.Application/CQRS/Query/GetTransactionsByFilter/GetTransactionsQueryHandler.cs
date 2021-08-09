using AutoMapper;
using MediatR;
using Payment.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.Core.Application.CQRS.Query.GetTransactionsByFilter
{
    public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, GetTransactionsQueryResponse>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public GetTransactionsQueryHandler(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<GetTransactionsQueryResponse> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            var transactions = await _transactionRepository.GetTransactionsByFilterAsync(request.PageSize, request.PageIndex);

            var totalCount =await _transactionRepository.GetTotalCount(k=> k.Status == Domain.Enums.TransactionStatus.Authorized);

            var response = new GetTransactionsQueryResponse
            {
                Transactions = _mapper.Map<List<TransactionModel>>(transactions),
                TotalCount = totalCount
            };
            return response;
        }
    }
}
