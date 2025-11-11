using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using System.Text.Json.Serialization;

namespace Market.Application.Modules.Reports.Comments.Commands.Update;

public sealed class UpdateCommentCommand : IRequest<Unit>
{
    [JsonIgnore] public int Id { get; set; }
    public string? Text { get; set; }
}

