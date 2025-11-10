using MediatR;

namespace Market.Application.Modules.Civic.WatchList.Queries.GetById
{
    public sealed class GetWatchListByIdQuery : IRequest<GetWatchListByIdQueryDto>
    {
        public int Id { get; init; }
    }
}
