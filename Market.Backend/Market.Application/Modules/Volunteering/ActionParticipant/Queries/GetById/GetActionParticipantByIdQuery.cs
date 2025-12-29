using MediatR;

namespace Market.Application.Modules.Volunteering.ActionParticipants.Queries.GetById;

public sealed class GetActionParticipantByIdQuery : IRequest<GetActionParticipantByIdQueryDto>
{
    public int Id { get; init; }
}
