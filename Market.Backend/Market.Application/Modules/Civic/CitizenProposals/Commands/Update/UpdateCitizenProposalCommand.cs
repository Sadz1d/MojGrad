using MediatR;
using System.Text.Json.Serialization;

namespace Market.Application.Modules.Civic.CitizenProposals.Commands.Update;

public sealed class UpdateCitizenProposalCommand : IRequest<Unit>
{
    [JsonIgnore] public int Id { get; set; }     // id dolazi iz rute
    public required string Title { get; set; }
    public required string Text { get; set; }
    public bool? IsEnabled { get; set; }         // opcionalno; ako null -> ne diramo flag
}