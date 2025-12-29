using Market.Domain.Common;
using Market.Domain.Entities.Identity;

namespace Market.Domain.Entities.Volunteering;

public class ActionParticipantEntity : BaseEntity
{
    public int? ActionId { get; set; }          // was int
    public int? UserId { get; set; }            // was int
    public DateTime RegistrationDate { get; set; }

    public VolunteerActionEntity? Action { get; set; }   // was non-nullable
    public MarketUserEntity? User { get; set; }          // was non-nullable
}

