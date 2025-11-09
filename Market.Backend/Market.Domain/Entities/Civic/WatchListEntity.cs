using Market.Domain.Common;
using Market.Domain.Entities.Identity;
using Market.Domain.Entities.Reports;

namespace Market.Domain.Entities.Civic;

public class WatchListEntity : BaseEntity
{
    public int UserId { get; set; }
    public int CategoryId { get; set; } // ProblemCategory
    public DateTime DateAdded { get; set; }

    public MarketUserEntity User { get; set; } = default!;
    public ProblemCategoryEntity Category { get; set; } = default!;
}
