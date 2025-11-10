namespace Market.Application.Modules.Civic.CitizenProposals.Queries.List;


public sealed class ListCitizenProposalsQueryDto
{
    public required int Id { get; init; }
    public required string Title { get; init; }       // Naziv prijedloga
    public required string AuthorName { get; init; }  // Ime korisnika (iz User)
    public required DateTime PublicationDate { get; init; } // Datum objave
    public string? ShortText { get; init; }           // Prvih n karaktera teksta
    //public required bool IsEnabled { get; init; }
}
