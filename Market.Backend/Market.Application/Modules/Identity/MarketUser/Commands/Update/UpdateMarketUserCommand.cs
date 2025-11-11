using MediatR;
using System.Text.Json.Serialization;

namespace Market.Application.Modules.Identity.Users.Commands.Update;

public sealed class UpdateMarketUserCommand : IRequest<Unit>
{
    [JsonIgnore]
    public int Id { get; set; } // ID dolazi iz rute

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PasswordHash { get; init; }  // <--- dodaj ovo
    public string? Email { get; set; }
    public bool? IsAdmin { get; set; }
    public bool? IsManager { get; set; }
    public bool? IsEmployee { get; set; }
    public bool? IsEnabled { get; set; }
}
