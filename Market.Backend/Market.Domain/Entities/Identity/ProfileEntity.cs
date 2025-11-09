using Market.Domain.Common;

namespace Market.Domain.Entities.Identity;

public sealed class ProfileEntity : BaseEntity
{
    public int UserId { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? ProfilePicture { get; set; }
    public string? BiographyText { get; set; }

    public MarketUserEntity User { get; set; } = null!;
}
