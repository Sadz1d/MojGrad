using Market.Domain.Common;

namespace Market.Domain.Entities.Events;

public class EventCalendarEntity : BaseEntity
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime EventDate { get; set; }
    public string? EventType { get; set; }
    public bool IsEnabled { get; set; } = true;


    public static class Constraints
    {
        public const int NameMaxLength = 150;
        public const int DescriptionMaxLength = 1000;
        public const int TypeMaxLength = 50;
    }
}
