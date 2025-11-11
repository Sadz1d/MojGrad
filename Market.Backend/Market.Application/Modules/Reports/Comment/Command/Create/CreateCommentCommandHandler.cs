using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Market.Domain.Entities.Reports;

namespace Market.Application.Modules.Reports.Comments.Commands.Create;

public sealed class CreateCommentCommandHandler
    : IRequestHandler<CreateCommentCommand, int>
{
    private readonly IAppDbContext _ctx;
    public CreateCommentCommandHandler(IAppDbContext ctx) => _ctx = ctx;

    public async Task<int> Handle(CreateCommentCommand request, CancellationToken ct)
    {
        var entity = new CommentEntity
        {
            ReportId = request.ReportId,
            UserId = request.UserId,
            Text = request.Text.Trim(),
            PublicationDate = DateTime.UtcNow
        };

        _ctx.Comments.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return entity.Id;
    }
}

