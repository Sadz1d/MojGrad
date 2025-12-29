using MediatR;

namespace Market.Application.Modules.Volunteering.VolunteerActions.Queries.GetById;

public sealed class GetVolunteerActionByIdQuery : IRequest<GetVolunteerActionByIdQueryDto>
{
    public int Id { get; init; }
}
