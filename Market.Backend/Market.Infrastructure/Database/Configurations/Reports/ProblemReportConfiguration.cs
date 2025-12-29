using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.Domain.Entities.Reports;

public class ProblemReportConfiguration
    : IEntityTypeConfiguration<ProblemReportEntity>
{
    public void Configure(EntityTypeBuilder<ProblemReportEntity> builder)
    {
        builder.ToTable("ProblemReports");

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(ProblemReportEntity.Constraints.TitleMaxLength);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(ProblemReportEntity.Constraints.DescriptionMaxLength);

        builder.HasOne(x => x.User)
            .WithMany(u => u.Reports)
            .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.Category)
            .WithMany(c => c.Reports)
            .HasForeignKey(x => x.CategoryId);

        builder.HasOne(x => x.Status)
            .WithMany(s => s.Reports)
            .HasForeignKey(x => x.StatusId);
    }
}
