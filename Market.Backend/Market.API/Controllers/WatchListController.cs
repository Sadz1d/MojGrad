using Market.Application.Modules.Civic.WatchList.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers
{
    [ApiController]
    [Route("api/civic/watch-list")]
    public sealed class WatchListController : ControllerBase
    {
        private readonly ISender sender;
        public WatchListController(ISender sender) => this.sender = sender;

        // GET /api/civic/watch-list?userId=1&categoryId=2&search=road&page=1&pageSize=20
        [HttpGet]
        public async Task<PageResult<ListWatchListQueryDto>> List(
            [FromQuery] ListWatchListQuery query,
            CancellationToken ct)
        {
            var result = await sender.Send(query, ct);
            return result;
        }
    }
}
