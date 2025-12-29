using MediatR;

namespace Market.Application.Modules.Events.Commands.Status.Enable;

public sealed class EnableEventCalendarCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}
