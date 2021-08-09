using MediatR;
using Payment.Core.Application.Exceptions;
using Payment.Core.Domain.Entities;
using Payment.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.Core.Application.CQRS.Command.Void
{
    public class VoidCommandHandler : IRequestHandler<VoidCommand, VoidResponse>
    {
        private readonly ITransactionRepository _transactionRepository;

        public VoidCommandHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<VoidResponse> Handle(VoidCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionRepository.GetAsync(request.Id, request.OrderReference);
            if (transaction == null)
            {
                throw new NotFoundException(nameof(Transaction), request.Id);
            }

            transaction.Status = Domain.Enums.TransactionStatus.Voided;
            _transactionRepository.Update(transaction);
            await _transactionRepository.UnitofWork.SaveEntitiesAsync();
            return new VoidResponse
            {
                Id = transaction.PaymentId,
                Status = transaction.Status
            };
        }
    }
}
