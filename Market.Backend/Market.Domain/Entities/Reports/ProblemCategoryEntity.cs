using Market.Domain.Common;
namespace Market.Domain.Entities.Reports;

public class ProblemCategoryEntity : BaseEntity
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public IReadOnlyCollection<ProblemReportEntity> Reports { get; private set; } = new List<ProblemReportEntity>();

    public static class Constraints
    {
        public const int NameMaxLength = 100;
        public const int DescriptionMaxLength = 500;
    }
}
