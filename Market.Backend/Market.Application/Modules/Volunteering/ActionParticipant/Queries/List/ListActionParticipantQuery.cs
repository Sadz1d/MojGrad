using MediatR;

namespace Market.Application.Modules.Volunteering.ActionParticipants.Queries.List;

public sealed class ListActionParticipantsQuery : BasePagedQuery<ListActionParticipantsQueryDto>
{
    public string? Search { get; init; }    // filtriranje po imenu korisnika / nazivu akcije
    public int? UserId { get; init; }       // filtriranje po korisniku
    public int? ActionId { get; init; }     // filtriranje po akciji
}
