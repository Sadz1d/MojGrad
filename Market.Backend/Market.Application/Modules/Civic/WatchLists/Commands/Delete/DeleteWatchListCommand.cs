using MediatR;

namespace Market.Application.Modules.Civic.WatchList.Commands.Delete
{
    public sealed class DeleteWatchListCommand : IRequest<Unit>
    {
        public required int Id { get; init; }
    }
}
