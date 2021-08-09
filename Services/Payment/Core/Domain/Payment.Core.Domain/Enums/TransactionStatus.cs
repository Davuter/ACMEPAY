using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Core.Domain.Enums
{
    public enum TransactionStatus
    {
        Authorized,
        Captured,
        Voided
    }
}
