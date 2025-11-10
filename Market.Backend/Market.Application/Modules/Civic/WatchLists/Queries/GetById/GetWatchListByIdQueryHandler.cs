using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Civic;
using Market.Application.Common.Exceptions;

namespace Market.Application.Modules.Civic.WatchList.Queries.GetById
{
    public sealed class GetWatchListByIdQueryHandler
        : IRequestHandler<GetWatchListByIdQuery, GetWatchListByIdQueryDto>
    {
        private readonly IAppDbContext _ctx;
        public GetWatchListByIdQueryHandler(IAppDbContext ctx) => _ctx = ctx;

        public async Task<GetWatchListByIdQueryDto> Handle(
            GetWatchListByIdQuery request, CancellationToken ct)
        {
            var item = await _ctx.WatchLists
                .AsNoTracking()
                .Include(w => w.User)
                .Include(w => w.Category)
                .Where(w => w.Id == request.Id)
                .Select(w => new GetWatchListByIdQueryDto
                {
                    Id = w.Id,
                    UserId = w.UserId,
                    CategoryId = w.CategoryId,
                    UserName = w.User != null
                        ? (w.User.FirstName + " " + w.User.LastName).Trim()
                        : "Anonimno",
                    CategoryName = w.Category.Name,
                    DateAdded = w.DateAdded
                })
                .FirstOrDefaultAsync(ct);

            if (item == null)
                throw new MarketNotFoundException($"WatchList item with Id {request.Id} not found.");

            return item;
        }
    }
}
