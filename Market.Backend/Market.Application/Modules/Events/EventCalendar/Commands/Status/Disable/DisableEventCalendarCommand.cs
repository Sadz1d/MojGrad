using MediatR;

namespace Market.Application.Modules.Events.Commands.Status.Disable;

public sealed class DisableEventCalendarCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}

