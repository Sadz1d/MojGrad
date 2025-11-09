using Market.Domain.Common;
using Market.Domain.Entities.Identity;

namespace Market.Domain.Entities.Volunteering;

public class VolunteerActionEntity : BaseEntity
{
    public int? VolunteerId { get; set; } // organizator (User)
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public DateTime EventDate { get; set; }
    public int MaxParticipants { get; set; }

    public MarketUserEntity? Volunteer { get; set; } = default!;
    public IReadOnlyCollection<ActionParticipantEntity> Participants { get; private set; } = new List<ActionParticipantEntity>();

    public static class Constraints
    {
        public const int NameMaxLength = 150;
        public const int DescriptionMaxLength = 1000;
        public const int LocationMaxLength = 200;
    }
}
