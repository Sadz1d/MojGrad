using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.Domain.Entities.Reports;

public class ProblemStatusConfiguration
    : IEntityTypeConfiguration<ProblemStatusEntity>
{
    public void Configure(EntityTypeBuilder<ProblemStatusEntity> builder)
    {
        builder.ToTable("ProblemStatuses");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(ProblemStatusEntity.Constraints.NameMaxLength);
    }
}
