using Market.Domain.Common;
using Market.Domain.Entities.Identity; // zbog Worker(User)
namespace Market.Domain.Entities.Reports;

public class TaskEntity : BaseEntity
{
    public int ReportId { get; set; }
    public int WorkerId { get; set; }
    public DateTime? AssignmentDate { get; set; }
    public DateTime? Deadline { get; set; }
    public string TaskStatus { get; set; } = default!; // ili enum

    public ProblemReportEntity Report { get; set; } = default!;
    public MarketUserEntity Worker { get; set; } = default!;
    public ProofOfResolutionEntity? Proof { get; set; }

    public static class Constraints
    {
        public const int StatusMaxLength = 50;
    }
}
