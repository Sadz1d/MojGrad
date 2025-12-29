using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.Domain.Entities.Rewards;

public class AssignedRewardConfiguration
    : IEntityTypeConfiguration<AssignedRewardEntity>
{
    public void Configure(EntityTypeBuilder<AssignedRewardEntity> builder)
    {
        builder.ToTable("AssignedRewards");

        builder.HasOne(x => x.User)
            .WithMany(u => u.AssignedRewards)
            .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.Reward)
            .WithMany(r => r.Assignments)
            .HasForeignKey(x => x.RewardId);
    }
}
