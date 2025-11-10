namespace Market.Application.Modules.Civic.WatchList.Queries.List
{
    public sealed class ListWatchListQueryDto
    {
        public required int Id { get; init; }
        public required string CategoryName { get; init; } // Naziv kategorije (iz ProblemCategory)
        public required string UserName { get; init; }     // Ime korisnika (iz User)
        public required DateTime DateAdded { get; init; }  // Datum dodavanja
    }
}
