using Market.Domain.Common;
using Market.Domain.Entities.Identity; // zbog UserEntity
namespace Market.Domain.Entities.Reports;

public class ProblemReportEntity : BaseEntity
{
    public string Title { get; set; } = default!;
    public int UserId { get; set; }
    public string Description { get; set; } = default!;
    public string? Location { get; set; }
    public int CategoryId { get; set; }
    public int StatusId { get; set; }
    public DateTime CreationDate { get; set; }

    public MarketUserEntity User { get; set; } = default!;
    public ProblemCategoryEntity Category { get; set; } = default!;
    public ProblemStatusEntity Status { get; set; } = default!;
    public IReadOnlyCollection<CommentEntity> Comments { get; private set; } = new List<CommentEntity>();
    public IReadOnlyCollection<TaskEntity> Tasks { get; private set; } = new List<TaskEntity>();
    public IReadOnlyCollection<RatingEntity> Ratings { get; private set; } = new List<RatingEntity>();

    public static class Constraints
    {
        public const int TitleMaxLength = 150;
        public const int LocationMaxLength = 200;
        public const int DescriptionMaxLength = 2000;
    }
}
