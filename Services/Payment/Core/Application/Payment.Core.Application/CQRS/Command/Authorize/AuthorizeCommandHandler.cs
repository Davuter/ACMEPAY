using AutoMapper;
using MediatR;
using Payment.Core.Application.Exceptions;
using Payment.Core.Domain.Entities;
using Payment.Core.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.Core.Application.CQRS.Command.Authorize
{
    public class AuthorizeCommandHandler : IRequestHandler<AuthorizeCommand, AuthorizeResponse>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        public AuthorizeCommandHandler(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<AuthorizeResponse> Handle(AuthorizeCommand request, CancellationToken cancellationToken)
        {

            bool isValidCardExpirationDate = (DateTime.Now.Year * 10)  + DateTime.Now.Month < (request.CardExpirationYear * 10) + request.CardExpirationMonth;

            if (!isValidCardExpirationDate)
            {
                throw new NotValidDataException(nameof(AuthorizeCommand) + "-> " +
                    string.Join("-",new string[] { nameof(AuthorizeCommand.CardExpirationMonth), nameof(AuthorizeCommand.CardExpirationYear)}),
                    new object[] { request.CardExpirationMonth, request.CardExpirationYear });
            }

            var transaction = _mapper.Map<Transaction>(request);
            var paymentId = await _transactionRepository.Add(transaction);
            await _transactionRepository.UnitofWork.SaveEntitiesAsync();
            var response = new AuthorizeResponse
            {
                Id = paymentId,
                Status = Domain.Enums.TransactionStatus.Authorized
            };
            return response;
        }
    }
}
