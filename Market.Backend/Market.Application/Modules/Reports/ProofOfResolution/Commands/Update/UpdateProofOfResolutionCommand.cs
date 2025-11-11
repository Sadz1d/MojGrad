using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using System.Text.Json.Serialization;

namespace Market.Application.Modules.Reports.ProofOfResolution.Commands.Update;

public sealed class UpdateProofOfResolutionCommand : IRequest<Unit>
{
    [JsonIgnore] public int Id { get; set; }
    public int? TaskId { get; set; }
    public DateTime? UploadDate { get; set; }
}

