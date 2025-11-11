using MediatR;
using System.Text.Json.Serialization;

namespace Market.Application.Modules.Identity.Roles.Commands.Update;

public sealed class UpdateRoleCommand : IRequest<Unit>
{
    [JsonIgnore]
    public int Id { get; set; }  // dolazi iz rute

    public required string Name { get; set; }
    public string? Description { get; set; }
}
