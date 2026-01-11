using Market.Domain.Entities.Civic;
using Market.Domain.Entities.Events;
using Market.Domain.Entities.Media;
using Market.Domain.Entities.Reports;
using Market.Domain.Entities.Rewards;
using Market.Domain.Entities.Surveys;
using Market.Domain.Entities.Volunteering;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Market.Domain.Entities.Identity;


namespace Market.Application.Abstractions;

// Application layer
public interface IAppDbContext
{
    // ===== Identity =====
    DbSet<MarketUserEntity> Users { get; }
    DbSet<RefreshTokenEntity> RefreshTokens { get; }
    DbSet<ProfileEntity> Profiles { get; }
    DbSet<RoleEntity> Roles { get; }
    DbSet<UserRoleEntity> UserRoles { get; }
    DbSet<PasswordResetToken> PasswordResetTokens { get; }



    // ===== Reports =====
    DbSet<ProblemReportEntity> ProblemReports { get; }
    DbSet<ProblemCategoryEntity> ProblemCategories { get; }
    DbSet<ProblemStatusEntity> ProblemStatuses { get; }
    DbSet<CommentEntity> Comments { get; }
    DbSet<TaskEntity> Tasks { get; }
    DbSet<ProofOfResolutionEntity> ProofsOfResolution { get; }
    DbSet<RatingEntity> Ratings { get; }

    // ===== Media =====
    DbSet<MediaAttachmentEntity> MediaAttachments { get; }
    DbSet<MediaLinkEntity> MediaLinks { get; }

    // ===== Surveys =====
    DbSet<SurveyEntity> Surveys { get; }
    DbSet<SurveyResponseEntity> SurveyResponses { get; }

    // ===== Rewards =====
    DbSet<RewardEntity> Rewards { get; }
    DbSet<AssignedRewardEntity> AssignedRewards { get; }

    // ===== Civic =====
    DbSet<CitizenProposalEntity> CitizenProposals { get; }
    DbSet<WatchListEntity> WatchLists { get; }

    // ===== Events =====
    DbSet<EventCalendarEntity> EventsCalendar { get; }

    // ===== Volunteering =====
    DbSet<VolunteerActionEntity> VolunteerActions { get; }
    DbSet<ActionParticipantEntity> ActionParticipants { get; }

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken ct);
}