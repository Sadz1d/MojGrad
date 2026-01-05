// MarketUserEntity.cs
using Market.Domain.Common;
using Market.Domain.Entities.Civic;
using Market.Domain.Entities.Reports;
using Market.Domain.Entities.Rewards;
using Market.Domain.Entities.Surveys;
using Market.Domain.Entities.Volunteering;


namespace Market.Domain.Entities.Identity;

public sealed class MarketUserEntity : BaseEntity
{
    public string? FirstName { get; set; } = default!;
    public string? LastName { get; set; } = default!;
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsManager { get; set; }
    public bool IsEmployee { get; set; }
    public int TokenVersion { get; set; } = 0;// For global revocation
    public bool IsEnabled { get; set; }

    public DateTime RegistrationDate { get; set; }
    public int Points { get; set; }
    public ProfileEntity? Profile { get; set; }

    public IReadOnlyCollection<UserRoleEntity> UserRoles { get; private set; } = new List<UserRoleEntity>();

    public IReadOnlyCollection<ProblemReportEntity> Reports { get; private set; } = new List<ProblemReportEntity>();
    public IReadOnlyCollection<CommentEntity> Comments { get; private set; } = new List<CommentEntity>();
    public IReadOnlyCollection<SurveyResponseEntity> SurveyResponses { get; private set; } = new List<SurveyResponseEntity>();
    public IReadOnlyCollection<AssignedRewardEntity> AssignedRewards { get; private set; } = new List<AssignedRewardEntity>();
    public IReadOnlyCollection<WatchListEntity> WatchList { get; private set; } = new List<WatchListEntity>();
    public IReadOnlyCollection<TaskEntity> TasksAsWorker { get; private set; } = new List<TaskEntity>();
    public IReadOnlyCollection<RatingEntity> Ratings { get; private set; } = new List<RatingEntity>();
    public IReadOnlyCollection<ActionParticipantEntity> ActionParticipations { get; private set; } = new List<ActionParticipantEntity>();


    public static class Constraints
    {
        public const int NameMaxLength = 100;
        public const int EmailMaxLength = 256;
        public const int PasswordHashMaxLength = 256;
        public const int UserTypeMaxLength = 50;
    }

    public ICollection<RefreshTokenEntity> RefreshTokens { get; private set; } = new List<RefreshTokenEntity>();
}
