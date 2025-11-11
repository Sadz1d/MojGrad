using MediatR;

namespace Market.Application.Modules.Events.EventCalendar.Commands.Delete;

public sealed class DeleteEventCalendarCommand : IRequest<Unit>
{
    public required int Id { get; init; }
}