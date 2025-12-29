using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.Domain.Entities.Volunteering;

public class VolunteerActionConfiguration
    : IEntityTypeConfiguration<VolunteerActionEntity>
{
    public void Configure(EntityTypeBuilder<VolunteerActionEntity> builder)
    {
        builder.ToTable("VolunteerActions");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(VolunteerActionEntity.Constraints.NameMaxLength);

        builder.Property(x => x.Location)
            .HasMaxLength(VolunteerActionEntity.Constraints.LocationMaxLength);
    }
}
