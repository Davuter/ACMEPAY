using Microsoft.EntityFrameworkCore;
using Payment.Core.Domain.Entities;
using Payment.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly PaymentDbContext _context;

        public TransactionRepository(PaymentDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitofWork UnitofWork
        {
            get { return _context; }
        }

        public async Task<Guid> Add(Transaction transaction)
        {
            transaction.PaymentId = Guid.NewGuid();
            transaction.CreateDate = DateTime.Now;
            var entityEntry = await _context.Transactions.AddAsync(transaction);
            return entityEntry.Entity.PaymentId;
        }

        public async Task<List<Transaction>> Filter(Expression<Func<Transaction, bool>> predicate, int pageSize, int pageIndex)
        {
            return await _context.Transactions.Where(predicate).Skip(pageSize * (pageIndex - 1))
                 .Take(pageSize).ToListAsync();
        }

        public async Task<Transaction> GetAsync(Guid Id, string orderReferenceNumber)
        {
            return await _context.Transactions.FirstOrDefaultAsync(k => k.PaymentId == Id && k.OrderReferenceNumber == orderReferenceNumber);
        }

        public async Task<int> GetTotalCount(Expression<Func<Transaction, bool>> predicate)
        {
            return await _context.Transactions.CountAsync(predicate);
        }

        public async Task<List<Transaction>> GetTransactionsByFilterAsync(int pageSize, int pageIndex)
        {
            return await _context.Transactions.Skip(pageSize * (pageIndex - 1))
                 .Take(pageSize).ToListAsync();
        }

        public void Update(Transaction transaction)
        {
            _context.Entry(transaction).State = EntityState.Modified;
        }
    }
}
