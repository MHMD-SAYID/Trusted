using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trusted.Core.Entities.Games;

namespace Trusted.Infrastructure.EFConfiguration
{
    public class DealConfiguration : IEntityTypeConfiguration<Deal>
    {
        public void Configure(EntityTypeBuilder<Deal> builder)
        {
            builder.HasKey(d => d.Id);

            builder.HasOne(d => d.Account)
                .WithMany(a => a.Deals)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Buyer)
                .WithMany(u => u.Deals)
                .HasForeignKey(d => d.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.SelectedMediator)
                .WithMany(u => u.MediatedDeals)
                .HasForeignKey(d => d.SelectedMediatorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(d => d.Status)
                .HasConversion<string>()
                .HasMaxLength(20);
        }
    }
}

