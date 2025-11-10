namespace Market.Application.Modules.Civic.WatchList.Queries.List
{
    public sealed class ListWatchListQuery : BasePagedQuery<ListWatchListQueryDto>
    {
        public int? UserId { get; init; } // možeš filtrirati po korisniku
        public int? CategoryId { get; init; } // opcionalno filtriranje po kategoriji
        public string? Search { get; init; } // ako kasnije želiš tražiti po imenu kategorije
    }
}
