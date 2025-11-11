using MediatR;
using System.Text.Json.Serialization;

namespace Market.Application.Modules.Identity.Profiles.Commands.Update;

public sealed class UpdateProfileCommand : IRequest<Unit>
{
    [JsonIgnore]
    public int Id { get; set; } // dolazi iz rute

    public string? Address { get; init; }
    public string? Phone { get; init; }
    public string? ProfilePicture { get; init; }
    public string? BiographyText { get; init; }
}
