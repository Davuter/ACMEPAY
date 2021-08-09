using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Core.Domain.Entities
{
    public abstract class BaseEntity
    {
        public DateTime CreateDate { get; set; }
    }
}
