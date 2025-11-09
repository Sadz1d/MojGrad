using Market.Domain.Common;
using Market.Domain.Entities.Identity; // zbog UserEntity
namespace Market.Domain.Entities.Reports;

public class CommentEntity : BaseEntity
{
    public int ReportId { get; set; }
    public int UserId { get; set; }
    public string Text { get; set; } = default!;
    public DateTime PublicationDate { get; set; }

    public ProblemReportEntity Report { get; set; } = default!;
    public MarketUserEntity User { get; set; } = default!;

    public static class Constraints
    {
        public const int TextMaxLength = 2000;
    }
}
