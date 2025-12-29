using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.Domain.Entities.Rewards;

public class RewardConfiguration
    : IEntityTypeConfiguration<RewardEntity>
{
    public void Configure(EntityTypeBuilder<RewardEntity> builder)
    {
        builder.ToTable("Rewards");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(RewardEntity.Constraints.NameMaxLength);

        builder.Property(x => x.Description)
            .HasMaxLength(RewardEntity.Constraints.DescriptionMaxLength);
    }
}
