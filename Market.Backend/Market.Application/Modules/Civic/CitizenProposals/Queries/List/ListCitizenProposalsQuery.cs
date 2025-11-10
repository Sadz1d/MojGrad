namespace Market.Application.Modules.Civic.CitizenProposals.Queries.List;

public sealed class ListCitizenProposalsQuery : BasePagedQuery<ListCitizenProposalsQueryDto>
{
    public string? Search { get; init; }
    //public bool? OnlyEnabled { get; init; }
}
