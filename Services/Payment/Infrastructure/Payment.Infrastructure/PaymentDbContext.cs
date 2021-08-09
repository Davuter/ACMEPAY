using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Payment.Core.Domain.Entities;
using Payment.Core.Domain.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Payment.Infrastructure
{
    public class PaymentDbContext : DbContext, IUnitofWork
    {
        public PaymentDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
            System.Diagnostics.Debug.WriteLine("PaymentDbContext::ctor ->" + this.GetHashCode());
        }

        public DbSet<Transaction> Transactions { get; set; }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await base.SaveChangesAsync(cancellationToken);

            return true;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentDbContext).Assembly);
        }
    }



    public class PaymentContextDesignFactory : IDesignTimeDbContextFactory<PaymentDbContext>
    {
        public PaymentDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PaymentDbContext>()
                .UseSqlServer("Server=.;Initial Catalog=AcmePay.PaymentDb;Integrated Security=true");

            return new PaymentDbContext(optionsBuilder.Options);
        }
    }
}
