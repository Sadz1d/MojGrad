using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.Domain.Entities.Volunteering;

public class ActionParticipantConfiguration
    : IEntityTypeConfiguration<ActionParticipantEntity>
{
    public void Configure(EntityTypeBuilder<ActionParticipantEntity> builder)
    {
        builder.ToTable("ActionParticipants");

        builder.HasOne(x => x.Action)
            .WithMany(a => a.Participants)
            .HasForeignKey(x => x.ActionId);

        builder.HasOne(x => x.User)
            .WithMany(u => u.ActionParticipations)
            .HasForeignKey(x => x.UserId);
    }
}

