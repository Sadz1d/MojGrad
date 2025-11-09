using Market.Domain.Common;
using Market.Domain.Entities.Identity; // zbog UserEntity
namespace Market.Domain.Entities.Reports;

public class RatingEntity : BaseEntity
{
    public int UserId { get; set; }
    public int ReportId { get; set; }
    public int Rating { get; set; }
    public string? RatingComment { get; set; }

    public MarketUserEntity User { get; set; } = default!;
    public ProblemReportEntity Report { get; set; } = default!;

    public static class Constraints
    {
        public const int CommentMaxLength = 1000;
    }
}
