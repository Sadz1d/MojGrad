using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using System.Text.Json.Serialization;

namespace Market.Application.Modules.Media.Commands.Update;

public sealed class UpdateMediaAttachmentCommand : IRequest<Unit>
{
    [JsonIgnore] public int Id { get; set; }          // ID dolazi iz rute
    public string? FileUrl { get; set; }              // opcionalno - ako null, ne mijenja se
    public string? MimeType { get; set; }             // opcionalno - ako null, ne mijenja se
    public long? SizeBytes { get; set; }              // opcionalno - ako null, ne mijenja se
}

