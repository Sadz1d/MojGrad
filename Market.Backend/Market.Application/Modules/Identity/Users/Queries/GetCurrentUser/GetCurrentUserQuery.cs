using MediatR;

namespace Market.Application.Modules.Identity.Users.Queries.GetCurrentUser;

public sealed record GetCurrentUserQuery(int UserId)
    : IRequest<GetCurrentUserQueryDto>;
