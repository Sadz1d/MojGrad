using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.Domain.Entities.Surveys;

public class SurveyConfiguration
    : IEntityTypeConfiguration<SurveyEntity>
{
    public void Configure(EntityTypeBuilder<SurveyEntity> builder)
    {
        builder.ToTable("Surveys");

        builder.Property(x => x.Question)
            .IsRequired()
            .HasMaxLength(SurveyEntity.Constraints.QuestionMaxLength);
    }
}
