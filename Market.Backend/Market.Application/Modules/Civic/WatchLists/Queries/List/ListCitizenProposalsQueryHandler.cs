using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Civic;

namespace Market.Application.Modules.Civic.WatchList.Queries.List
{
    public sealed class ListWatchListQueryHandler
        : IRequestHandler<ListWatchListQuery, PageResult<ListWatchListQueryDto>>
    {
        private readonly IAppDbContext _ctx;

        public ListWatchListQueryHandler(IAppDbContext ctx) => _ctx = ctx;

        public async Task<PageResult<ListWatchListQueryDto>> Handle(
            ListWatchListQuery request, CancellationToken ct)
        {
            IQueryable<WatchListEntity> q = _ctx.WatchLists
                .AsNoTracking()
                .Include(w => w.User)
                .Include(w => w.Category);

            // Filtriranje po korisniku ako je zadano
            if (request.UserId.HasValue)
                q = q.Where(w => w.UserId == request.UserId.Value);

            // Filtriranje po kategoriji ako je zadano
            if (request.CategoryId.HasValue)
                q = q.Where(w => w.CategoryId == request.CategoryId.Value);

            // Pretraga (npr. po imenu kategorije)
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var term = request.Search.Trim().ToLower();
                q = q.Where(w =>
                    w.Category.Name.ToLower().Contains(term) ||
                    (w.User.FirstName + " " + w.User.LastName).ToLower().Contains(term));
            }

            var projected = q
                .OrderByDescending(w => w.DateAdded)
                .Select(w => new ListWatchListQueryDto
                {
                    Id = w.Id,
                    CategoryName = w.Category.Name,
                    UserName = w.User != null
                        ? (w.User.FirstName + " " + w.User.LastName).Trim()
                        : "Anonimno",
                    DateAdded = w.DateAdded
                });

            return await PageResult<ListWatchListQueryDto>
                .FromQueryableAsync(projected, request.Paging, ct);
        }
    }
}
