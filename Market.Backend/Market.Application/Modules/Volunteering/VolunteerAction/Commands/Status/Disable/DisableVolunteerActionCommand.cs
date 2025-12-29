using MediatR;

namespace Market.Application.Modules.Volunteering.Commands.Status.Disable;

public sealed class DisableVolunteerActionCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}
