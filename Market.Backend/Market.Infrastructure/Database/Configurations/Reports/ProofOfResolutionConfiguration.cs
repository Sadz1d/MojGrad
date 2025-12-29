using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.Domain.Entities.Reports;

public class ProofOfResolutionConfiguration
    : IEntityTypeConfiguration<ProofOfResolutionEntity>
{
    public void Configure(EntityTypeBuilder<ProofOfResolutionEntity> builder)
    {
        builder.ToTable("ProofsOfResolution");

        builder.HasOne(x => x.Task)
            .WithOne(t => t.Proof)
            .HasForeignKey<ProofOfResolutionEntity>(x => x.TaskId);
    }
}
