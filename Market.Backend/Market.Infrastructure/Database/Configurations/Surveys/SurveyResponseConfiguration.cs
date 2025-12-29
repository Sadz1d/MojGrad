using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.Domain.Entities.Surveys;

public class SurveyResponseConfiguration
    : IEntityTypeConfiguration<SurveyResponseEntity>
{
    public void Configure(EntityTypeBuilder<SurveyResponseEntity> builder)
    {
        builder.ToTable("SurveyResponses");

        builder.Property(x => x.ResponseText)
            .IsRequired()
            .HasMaxLength(SurveyResponseEntity.Constraints.ResponseMaxLength);
    }
}
