using Market.Domain.Common;
namespace Market.Domain.Entities.Reports;

public class ProofOfResolutionEntity : BaseEntity
{
    public int TaskId { get; set; }
    public DateTime UploadDate { get; set; }
    public TaskEntity Task { get; set; } = default!;
}
