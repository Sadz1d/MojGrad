using MediatR;

namespace Market.Application.Modules.Identity.Users.Commands.Delete;

public sealed class DeleteMarketUserCommand : IRequest<Unit>
{
    public int Id { get; init; }
}
