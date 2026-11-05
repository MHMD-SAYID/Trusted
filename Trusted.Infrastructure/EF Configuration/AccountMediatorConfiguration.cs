using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trusted.Core.Entities.Games;

namespace Trusted.Infrastructure.EFConfiguration
{
    public class AccountMediatorConfiguration : IEntityTypeConfiguration<AccountMediator>
    {
        public void Configure(EntityTypeBuilder<AccountMediator> builder)
        {
            builder.HasKey(am => new { am.AccountId, am.MediatorId });

            builder.HasOne(am => am.Account)
                .WithMany(a => a.AccountMediators)
                .HasForeignKey(am => am.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(am => am.Mediator)
                .WithMany(u => u.MediatedAccounts)
                .HasForeignKey(am => am.MediatorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

