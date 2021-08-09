using Payment.Core.Domain.Enums;
using System;

namespace Payment.Core.Domain.Entities
{
    public class Transaction: BaseEntity
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
