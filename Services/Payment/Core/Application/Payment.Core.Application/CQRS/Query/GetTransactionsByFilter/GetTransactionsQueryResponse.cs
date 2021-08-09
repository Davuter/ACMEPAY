using Payment.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;


namespace Payment.Core.Application.CQRS.Query.GetTransactionsByFilter
{
    public class GetTransactionsQueryResponse
    {
        public List<TransactionModel> Transactions { get; set; }

        public int TotalCount { get; set; }
    }

    public class TransactionModel
    {
        public Guid PaymentId { get; set; }
        public string OrderReferenceNumber { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CardHolderName { get; set; }
        public string CardPan { get; set; }
        public int CardExpirationMonth { get; set; }
        public int CardExpirationYear { get; set; }
        public int CardCvv { get; set; }
        public TransactionStatus Status { get; set; }
    }
}
