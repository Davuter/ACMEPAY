using AutoMapper;
using MediatR;
using Payment.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.Core.Application.CQRS.Command.Capture
{
    public class CaptureCommandHandler : IRequestHandler<CaptureCommand, CaptureResponse>
    {
        private readonly ITransactionRepository _transactionRepository;
        public CaptureCommandHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<CaptureResponse> Handle(CaptureCommand request, CancellationToken cancellationToken)
        {
            var transaction = await  _transactionRepository.GetAsync(request.Id, request.OrderReference);
            transaction.Status = Domain.Enums.TransactionStatus.Captured;
            _transactionRepository.Update(transaction);
            await _transactionRepository.UnitofWork.SaveEntitiesAsync(cancellationToken);

            return new CaptureResponse
            {
                Id = transaction.PaymentId,
                Status = Domain.Enums.TransactionStatus.Captured
            };
        }
    }
}
