using Market.Domain.Common;
using Market.Domain.Entities.Identity;

namespace Market.Domain.Entities.Media;

public class MediaAttachmentEntity : BaseEntity
{
    public int UploaderId { get; set; }
    public string FileUrl { get; set; } = default!;
    public string MimeType { get; set; } = default!;
    public long SizeBytes { get; set; }
    public DateTime CreatedAt { get; set; }

    public MarketUserEntity Uploader { get; set; } = default!;

    public static class Constraints
    {
        public const int UrlMaxLength = 500;
        public const int MimeMaxLength = 100;
    }
}
