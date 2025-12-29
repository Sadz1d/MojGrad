using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.Domain.Entities.Reports;
public class CommentConfiguration
    : IEntityTypeConfiguration<CommentEntity>
{
    public void Configure(EntityTypeBuilder<CommentEntity> builder)
    {
        builder.ToTable("Comments");

        builder.Property(x => x.Text)
            .IsRequired()
            .HasMaxLength(CommentEntity.Constraints.TextMaxLength);

        builder.HasOne(x => x.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.Report)
            .WithMany(r => r.Comments)
            .HasForeignKey(x => x.ReportId);
    }
}
