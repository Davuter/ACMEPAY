using Payment.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Core.Domain.Interfaces
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<Guid> Add(Transaction transaction);

        void Update(Transaction transaction);

        Task<Transaction> GetAsync(Guid Id, string orderReferenceNumber);

        Task<List<Transaction>> GetTransactionsByFilterAsync(int pageSize, int pageIndex);

        Task<List<Transaction>> Filter(Expression<Func<Transaction, bool>> predicate, int pageSize, int pageIndex);

        Task<int> GetTotalCount(Expression<Func<Transaction, bool>> predicate);
    }
}
