using MediatR;
using System.Text.Json.Serialization;

namespace Market.Application.Modules.Events.EventCalendar.Commands.Update
{
    public sealed class UpdateEventCalendarCommand : IRequest<Unit>
    {
        [JsonIgnore] public int Id { get; set; }   // Id dolazi iz rute
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required DateTime EventDate { get; set; }
        public string? EventType { get; set; }
    }
}
