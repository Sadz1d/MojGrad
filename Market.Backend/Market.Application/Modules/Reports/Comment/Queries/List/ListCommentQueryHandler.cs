using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities.Reports;


namespace Market.Application.Modules.Reports.Comment.Queries.List
{
    public sealed class ListCommentQueryHandler
        : IRequestHandler<ListCommentQuery, PageResult<ListCommentQueryDto>>
    {
        private readonly IAppDbContext _ctx;

        public ListCommentQueryHandler(IAppDbContext ctx) => _ctx = ctx;

        public async Task<PageResult<ListCommentQueryDto>> Handle(
            ListCommentQuery request, CancellationToken ct)
        {
            IQueryable<CommentEntity> q = _ctx.Comments
                .AsNoTracking()
                .Include(c => c.User)
                .Include(c => c.Report);

            if (request.ReportId.HasValue)
                q = q.Where(c => c.ReportId == request.ReportId.Value);

            if (request.UserId.HasValue)
                q = q.Where(c => c.UserId == request.UserId.Value);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var term = request.Search.Trim().ToLower();
                q = q.Where(c => c.Text.ToLower().Contains(term));
            }

            var projected = q
                .OrderByDescending(c => c.PublicationDate)
                .Select(c => new ListCommentQueryDto
                {
                    Id = c.Id,
                    ReportId = c.ReportId,
                    UserId = c.UserId,
                    UserName = c.User != null
                        ? (c.User.FirstName + " " + c.User.LastName).Trim()
                        : "Anonimno",
                    Text = c.Text,
                    ShortText = c.Text.Length > 120
                        ? c.Text.Substring(0, 120) + "..."
                        : c.Text,
                    PublicationDate = c.PublicationDate
                });

            return await PageResult<ListCommentQueryDto>
                .FromQueryableAsync(projected, request.Paging, ct);
        }
    }
}


