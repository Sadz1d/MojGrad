using MediatR;
using Market.Application.Modules.Identity.Profiles.Queries.GetById;

namespace Market.Application.Modules.Identity.Profiles.Queries.GetByUserId;

public sealed class GetProfileByUserIdQuery : IRequest<GetProfileByIdQueryDto>
{
    public int UserId { get; init; }
}