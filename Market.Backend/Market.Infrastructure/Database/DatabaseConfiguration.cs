using Market.Domain.Common;
using Market.Domain.Entities.Civic;
using Market.Domain.Entities.Reports;
using Market.Domain.Entities.Rewards;
using Market.Domain.Entities.Surveys;
using Market.Domain.Entities.Volunteering;
using Market.Infrastructure.Database.Seeders;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.X86;

namespace Market.Infrastructure.Database;

public partial class DatabaseContext
{
    private DateTime UtcNow => _clock.GetUtcNow().UtcDateTime;

    private void ApplyAuditAndSoftDelete()
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAtUtc = UtcNow;
                    entry.Entity.ModifiedAtUtc = null; // ili = UtcNow
                    entry.Entity.IsDeleted = false;
                    break;

                case EntityState.Modified:
                    entry.Entity.ModifiedAtUtc = UtcNow;
                    break;

                case EntityState.Deleted:
                    // soft-delete: set is Modified and IsDeleted
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.ModifiedAtUtc = UtcNow;
                    break;
            }
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
        configurationBuilder.Properties<decimal?>().HavePrecision(18, 2);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //bugfix 27.10.2025.nakon nastave - učitaj sve konfiguracije iz Infrastructure.Database.Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);



        modelBuilder.Entity<ProblemReportEntity>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reports)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.NoAction);       // <— NA USERS: NoAction

        modelBuilder.Entity<ProblemReportEntity>()
            .HasOne(r => r.Category)
            .WithMany(c => c.Reports)
            .HasForeignKey(r => r.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ProblemReportEntity>()
            .HasOne(r => r.Status)
            .WithMany(s => s.Reports)
            .HasForeignKey(r => r.StatusId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<CommentEntity>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);       // <— NA USERS: NoAction

        modelBuilder.Entity<CommentEntity>()
            .HasOne(c => c.Report)
            .WithMany(r => r.Comments)
            .HasForeignKey(c => c.ReportId)
            .OnDelete(DeleteBehavior.Cascade);        // <— unutar agregata OK

        modelBuilder.Entity<TaskEntity>()
            .HasOne(t => t.Worker)
            .WithMany(u => u.TasksAsWorker)
            .HasForeignKey(t => t.WorkerId)
            .OnDelete(DeleteBehavior.NoAction);       // <— NA USERS: NoAction

        modelBuilder.Entity<TaskEntity>()
            .HasOne(t => t.Report)
            .WithMany(r => r.Tasks)
            .HasForeignKey(t => t.ReportId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RatingEntity>()
            .HasOne(r => r.User)
            .WithMany(u => u.Ratings)                 // dodaj kolekciju u UserEntity ako je nema
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.NoAction);       // <— NA USERS: NoAction

        modelBuilder.Entity<RatingEntity>()
            .HasOne(r => r.Report)
            .WithMany(p => p.Ratings)
            .HasForeignKey(r => r.ReportId)
            .OnDelete(DeleteBehavior.Cascade);

        // ===== Civic =====

        modelBuilder.Entity<WatchListEntity>()
            .HasOne(w => w.User)
            .WithMany(u => u.WatchList)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // ===== Surveys =====

        modelBuilder.Entity<SurveyResponseEntity>()
            .HasOne(s => s.User)
            .WithMany(u => u.SurveyResponses)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // ===== Rewards =====

        modelBuilder.Entity<AssignedRewardEntity>()
            .HasOne(r => r.User)
            .WithMany(u => u.AssignedRewards)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    





    modelBuilder.Entity<ActionParticipantEntity>()
    .HasOne(ap => ap.Action)
    .WithMany(a => a.Participants)
    .HasForeignKey(ap => ap.ActionId)
    .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ActionParticipantEntity>()
            .HasOne(ap => ap.User)
            .WithMany()
            .HasForeignKey(ap => ap.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<VolunteerActionEntity>()
            .HasOne(va => va.Volunteer)
            .WithMany()
            .HasForeignKey(va => va.VolunteerId)
            .OnDelete(DeleteBehavior.NoAction);






        ApplyGlobalFielters(modelBuilder);

        StaticDataSeeder.Seed(modelBuilder); // static data
    }

    private void ApplyGlobalFielters(ModelBuilder modelBuilder)
    {
        // Apply a global filter to all entities inheriting from BaseEntity
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var prop = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                var compare = Expression.Equal(prop, Expression.Constant(false));
                var lambda = Expression.Lambda(compare, parameter);

                modelBuilder.Entity(entityType.ClrType)
                            .HasQueryFilter(lambda);
            }
        }
    }

    public override int SaveChanges()
    {
        ApplyAuditAndSoftDelete();

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        ApplyAuditAndSoftDelete();

        return base.SaveChangesAsync(cancellationToken);
    }
}