using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Domain.Entities.Civic;

namespace Market.Application.Modules.Civic.WatchList.Commands.Delete
{
    public sealed class DeleteWatchListCommandHandler
        : IRequestHandler<DeleteWatchListCommand, Unit>
    {
        private readonly IAppDbContext _ctx;

        public DeleteWatchListCommandHandler(IAppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Unit> Handle(DeleteWatchListCommand request, CancellationToken ct)
        {
            var item = await _ctx.WatchLists
                .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

            if (item == null)
                throw new MarketNotFoundException($"WatchList item with Id {request.Id} not found.");

            _ctx.WatchLists.Remove(item);
            await _ctx.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}
