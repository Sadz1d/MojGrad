using Market.Domain.Common;
namespace Market.Domain.Entities.Reports;

public class ProblemStatusEntity : BaseEntity
{
    public string Name { get; set; } = default!;
    public IReadOnlyCollection<ProblemReportEntity> Reports { get; private set; } = new List<ProblemReportEntity>();

    public static class Constraints
    {
        public const int NameMaxLength = 50;
    }
}
