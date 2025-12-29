using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.Domain.Entities.Reports;

public class RatingConfiguration
    : IEntityTypeConfiguration<RatingEntity>
{
    public void Configure(EntityTypeBuilder<RatingEntity> builder)
    {
        builder.ToTable("Ratings");

        builder.Property(x => x.Rating)
            .IsRequired();

        builder.Property(x => x.RatingComment)
            .HasMaxLength(RatingEntity.Constraints.CommentMaxLength);
    }
}
