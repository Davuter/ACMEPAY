using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Common.ViewModels
{
    public class AuthorizeResponse
    {
        public Guid Id { get; set; }

        public TransactionStatus Status { get; set; }
    }
}
