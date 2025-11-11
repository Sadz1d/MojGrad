using MediatR;

namespace Market.Application.Modules.Identity.Users.Commands.Create;

public sealed class CreateMarketUserCommand : IRequest<int>
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string PasswordHash { get; init; }
    public bool IsAdmin { get; init; } = false;
    public bool IsManager { get; init; } = false;
    public bool IsEmployee { get; init; } = false;
    public bool IsEnabled { get; init; } = true;
}
