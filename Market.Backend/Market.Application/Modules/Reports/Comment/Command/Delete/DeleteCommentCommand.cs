using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace Market.Application.Modules.Reports.Comments.Commands.Delete;

public sealed class DeleteCommentCommand : IRequest<Unit>
{
    public required int Id { get; init; }
}

