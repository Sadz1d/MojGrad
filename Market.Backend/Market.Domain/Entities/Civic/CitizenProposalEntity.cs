using Market.Domain.Common;
using Market.Domain.Entities.Identity;

namespace Market.Domain.Entities.Civic;

public class CitizenProposalEntity : BaseEntity
{
    public int UserId { get; set; }
    public string Title { get; set; } = default!;
    public string Text { get; set; } = default!;
    public DateTime PublicationDate { get; set; }
    public bool IsEnabled { get; set; } = true;


    public MarketUserEntity User { get; set; } = default!;

    public static class Constraints
    {
        public const int TitleMaxLength = 150;
        public const int TextMaxLength = 4000;
    }
}
