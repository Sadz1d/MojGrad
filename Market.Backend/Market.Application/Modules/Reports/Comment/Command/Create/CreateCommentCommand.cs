using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace Market.Application.Modules.Reports.Comments.Commands.Create;

public sealed class CreateCommentCommand : IRequest<int>
{
    public required int ReportId { get; init; }
    public required int UserId { get; init; }
    public required string Text { get; init; }
}

