using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Infrastructure.EntityConfiguration
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(i => i.PaymentId);
            builder.Property(i => i.OrderReferenceNumber).HasMaxLength(50);
            builder.Property(i => i.CardCvv).IsRequired();
            builder.Property(i => i.CardExpirationMonth).IsRequired();
            builder.Property(i => i.CardExpirationYear).IsRequired();
            builder.Property(i => i.CardHolderName).IsRequired();
            builder.Property(i => i.CardPan).IsRequired().HasMaxLength(16);
            builder.Property(i => i.OrderReferenceNumber).IsRequired();
            builder.Property(i => i.Amount).IsRequired();
            builder.Property(i => i.Currency).IsRequired();
            builder.Property(i => i.CreateDate).IsRequired();
        }
    }
}
