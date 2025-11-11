using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Market.Application.Common.Exceptions;
using Market.Application.Modules.Reports.Comment.Queries.GetById;

namespace Market.Application.Modules.Reports.Comment.Queries.GetById;

public sealed class GetCommentByIdQueryHandler
    : IRequestHandler<GetCommentByIdQuery, GetCommentByIdQueryDto>
{
    private readonly IAppDbContext _ctx;
    public GetCommentByIdQueryHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<GetCommentByIdQueryDto> Handle(GetCommentByIdQuery request, CancellationToken ct)
    {
        var dto = await _ctx.Comments
            .AsNoTracking()
            .Include(c => c.User)
            .Where(c => c.Id == request.Id)
            .Select(c => new GetCommentByIdQueryDto
            {
                Id = c.Id,
                ReportId = c.ReportId,
                UserId = c.UserId,
                Text = c.Text,
                UserName = c.User != null
                    ? (c.User.FirstName + " " + c.User.LastName).Trim()
                    : "Anonimno",
                PublicationDate = c.PublicationDate
            })
            .FirstOrDefaultAsync(ct);

        if (dto is null)
            throw new MarketNotFoundException($"Comment (Id={request.Id}) not found.");

        return dto;
    }
}

