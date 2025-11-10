namespace Market.Application.Modules.Civic.WatchList.Queries.GetById
{
    public sealed class GetWatchListByIdQueryDto
    {
        public required int Id { get; init; }
        public required int UserId { get; init; }
        public required int CategoryId { get; init; }
        public required string UserName { get; init; }       // Ime korisnika (iz User)
        public required string CategoryName { get; init; }   // Naziv kategorije (iz Category)
        public required DateTime DateAdded { get; init; }    // Datum dodavanja
    }
}
