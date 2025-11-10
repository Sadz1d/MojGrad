using MediatR;

namespace Market.Application.Modules.Civic.CitizenProposals.Commands.Create;

public sealed class CreateCitizenProposalCommand : IRequest<int>
{
    public required int UserId { get; init; }
    public required string Title { get; init; }
    public required string Text { get; init; }
    public bool? IsEnabled { get; init; } // opcionalno; default u handleru = true
}
