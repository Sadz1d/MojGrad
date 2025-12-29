using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.Domain.Entities.Reports;

public class TaskConfiguration
    : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.ToTable("Tasks");

        builder.Property(x => x.TaskStatus)
            .IsRequired()
            .HasMaxLength(TaskEntity.Constraints.StatusMaxLength);

        builder.HasOne(x => x.Report)
            .WithMany(r => r.Tasks)
            .HasForeignKey(x => x.ReportId);

        builder.HasOne(x => x.Worker)
            .WithMany(u => u.TasksAsWorker)
            .HasForeignKey(x => x.WorkerId);
    }
}
