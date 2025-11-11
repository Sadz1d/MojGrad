using MediatR;

namespace Market.Application.Modules.Identity.Profiles.Queries.GetById;

public sealed class GetProfileByIdQuery : IRequest<GetProfileByIdQueryDto>
{
    public int Id { get; init; }
}
