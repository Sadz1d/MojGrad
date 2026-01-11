using Market.Application.Abstractions;
using Market.Domain.Entities.Civic;
using Market.Domain.Entities.Events;
using Market.Domain.Entities.Media;
using Market.Domain.Entities.Reports;
using Market.Domain.Entities.Rewards;
using Market.Domain.Entities.Surveys;
using Market.Domain.Entities.Volunteering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Market.Infrastructure.Database;

public partial class DatabaseContext : DbContext, IAppDbContext
{
    
    public DbSet<MarketUserEntity> Users => Set<MarketUserEntity>();
    public DbSet<RefreshTokenEntity> RefreshTokens => Set<RefreshTokenEntity>();

    // ===== Identity =====
  
    public DbSet<ProfileEntity> Profiles => Set<ProfileEntity>();
    public DbSet<RoleEntity> Roles => Set<RoleEntity>();
    public DbSet<UserRoleEntity> UserRoles => Set<UserRoleEntity>();


    // ===== Reports (prijave) =====
    public DbSet<ProblemReportEntity> ProblemReports => Set<ProblemReportEntity>();
    public DbSet<ProblemCategoryEntity> ProblemCategories => Set<ProblemCategoryEntity>();
    public DbSet<ProblemStatusEntity> ProblemStatuses => Set<ProblemStatusEntity>();
    public DbSet<CommentEntity> Comments => Set<CommentEntity>();
    public DbSet<TaskEntity> Tasks => Set<TaskEntity>();
    public DbSet<ProofOfResolutionEntity> ProofsOfResolution => Set<ProofOfResolutionEntity>();
    public DbSet<RatingEntity> Ratings => Set<RatingEntity>();

    // ===== Media =====
    public DbSet<MediaAttachmentEntity> MediaAttachments => Set<MediaAttachmentEntity>();
    public DbSet<MediaLinkEntity> MediaLinks => Set<MediaLinkEntity>();

    // ===== Surveys =====
    public DbSet<SurveyEntity> Surveys => Set<SurveyEntity>();
    public DbSet<SurveyResponseEntity> SurveyResponses => Set<SurveyResponseEntity>();

    // ===== Rewards =====
    public DbSet<RewardEntity> Rewards => Set<RewardEntity>();
    public DbSet<AssignedRewardEntity> AssignedRewards => Set<AssignedRewardEntity>();

    // ===== Civic =====
    public DbSet<CitizenProposalEntity> CitizenProposals => Set<CitizenProposalEntity>();
    public DbSet<WatchListEntity> WatchLists => Set<WatchListEntity>();

    // ===== Events =====
    public DbSet<EventCalendarEntity> EventsCalendar => Set<EventCalendarEntity>();

    // ===== Volunteering =====
    public DbSet<VolunteerActionEntity> VolunteerActions => Set<VolunteerActionEntity>();
    public DbSet<ActionParticipantEntity> ActionParticipants => Set<ActionParticipantEntity>();

    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }


    public new DatabaseFacade Database => base.Database;

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return await base.Database.BeginTransactionAsync(cancellationToken);
    }





    private readonly TimeProvider _clock;
    public DatabaseContext(DbContextOptions<DatabaseContext> options, TimeProvider clock) : base(options)
    {
        _clock = clock;
    }







}