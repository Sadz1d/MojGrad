using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Market.Application.Modules.Media.Commands.Create;

public sealed class CreateMediaAttachmentCommand : IRequest<int>
{
    public required int UploaderId { get; init; }     // korisnik koji uploaduje
    public required string FileUrl { get; init; }     // putanja ili URL fajla
    public required string MimeType { get; init; }    // npr. "image/png"
    public required long SizeBytes { get; init; }     // veličina fajla u bajtovima
}
