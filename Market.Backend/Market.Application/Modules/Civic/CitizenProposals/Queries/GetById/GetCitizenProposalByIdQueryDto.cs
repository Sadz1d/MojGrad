namespace Market.Application.Modules.Civic.CitizenProposals.Queries.GetById;

public sealed class GetCitizenProposalByIdQueryDto
{
    public required int Id { get; init; }
    public required string Title { get; init; }
    public required string Text { get; init; }
    public required DateTime PublicationDate { get; init; }
    public required bool IsEnabled { get; init; }
    public required string AuthorName { get; init; }
}
