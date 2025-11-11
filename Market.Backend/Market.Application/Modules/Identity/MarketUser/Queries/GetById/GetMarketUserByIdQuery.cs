using MediatR;

namespace Market.Application.Modules.Identity.Users.Queries.GetById;

public sealed class GetMarketUserByIdQuery : IRequest<GetMarketUserByIdQueryDto>
{
    public int Id { get; init; }
}
