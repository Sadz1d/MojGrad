using Market.Application.Modules.Civic.WatchList.Queries.List;
using Market.Application.Modules.Civic.WatchList.Queries.GetById;
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

        // GET /api/civic/watch-list/{id}
        [HttpGet("{id:int}")]
        public async Task<GetWatchListByIdQueryDto> GetById(int id, CancellationToken ct)
        {
            var result = await sender.Send(new GetWatchListByIdQuery { Id = id }, ct);
            return result;
        }
    }
}
