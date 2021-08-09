using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Payment.Core.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IUnitofWork UnitofWork { get; }
    }
}
