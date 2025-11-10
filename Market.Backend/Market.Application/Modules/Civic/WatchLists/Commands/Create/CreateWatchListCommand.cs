using MediatR;

namespace Market.Application.Modules.Civic.WatchList.Commands.Create
{
    public sealed class CreateWatchListCommand : IRequest<int>
    {
        public required int UserId { get; init; }
        public required int CategoryId { get; init; }
        public DateTime? DateAdded { get; init; } // opcionalno; default u handleru = DateTime.UtcNow
    }
}

