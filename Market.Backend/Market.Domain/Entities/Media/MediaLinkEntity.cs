using Market.Domain.Common;

namespace Market.Domain.Entities.Media;

public class MediaLinkEntity : BaseEntity
{
    public int MediaId { get; set; }
    public string EntityType { get; set; } = default!; // npr. "ProblemReport","ProofOfResolution"
    public int EntityId { get; set; }

    public MediaAttachmentEntity Media { get; set; } = default!;

    public static class Constraints
    {
        public const int EntityTypeMaxLength = 100;
    }
}
