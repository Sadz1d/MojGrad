using MediatR;

namespace Market.Application.Modules.Identity.Profiles.Commands.Create;

public sealed class CreateProfileCommand : IRequest<int>
{
    public required int UserId { get; init; }
    public string? Address { get; init; }
    public string? Phone { get; init; }
    public string? ProfilePicture { get; init; }
    public string? BiographyText { get; init; }
}
