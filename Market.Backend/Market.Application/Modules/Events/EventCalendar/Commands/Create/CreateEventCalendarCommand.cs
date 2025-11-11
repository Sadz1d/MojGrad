using MediatR;

namespace Market.Application.Modules.Civic.Events.Commands.Create;

public sealed class CreateEventCalendarCommand : IRequest<int>
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required DateTime EventDate { get; init; }
    public string? EventType { get; init; }
}