using System;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.Core.Domain.Interfaces
{
    public interface IUnitofWork : IDisposable
    {
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
