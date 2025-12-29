using MediatR;

namespace Market.Application.Modules.Volunteering.Commands.Status.Enable;

public sealed class EnableVolunteerActionCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}
