using Market.Application.Modules.Reports.ProblemReport.Commands.Create;
using Market.Application.Modules.Reports.ProblemReport.Commands.Delete;
using Market.Application.Modules.Reports.ProblemReport.Commands.Import;
using Market.Application.Modules.Reports.ProblemReport.Commands.Update;
using Market.Application.Modules.Reports.ProblemReport.Commands.UploadImage;
using Market.Application.Modules.Reports.ProblemReport.Queries.GetById;
using Market.Application.Modules.Reports.ProblemReport.Queries.GetImage;
using Market.Application.Modules.Reports.ProblemReport.Queries.GetPaged;
using Market.Application.Modules.Reports.ProblemReport.Queries.List;
using Market.Application.Modules.Reports.ProblemReport.Dtos;
using Market.API.Models.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Market.API.Controllers;

[ApiController]
[Route("api/reports/problem-reports")]
public sealed class ProblemReportsController : ControllerBase
{
    private readonly ISender _sender;

    public ProblemReportsController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<PageResult<ListProblemReportQueryDto>> List(
        [FromQuery] ListProblemReportQuery query, CancellationToken ct)
        => await _sender.Send(query, ct);

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged(
        [FromQuery] GetPagedProblemReportsQuery query, CancellationToken ct)
        => Ok(await _sender.Send(query, ct));

    [HttpGet("{id:int}")]
    public async Task<GetProblemReportByIdQueryDto> GetById(int id, CancellationToken ct)
        => await _sender.Send(new GetProblemReportByIdQuery { Id = id }, ct);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProblemReportCommand command, CancellationToken ct)
    {
        var id = await _sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProblemReportCommand command, CancellationToken ct)
    {
        command.Id = id;
        await _sender.Send(command, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _sender.Send(new DeleteProblemReportCommand { Id = id }, ct);
        return NoContent();
    }

    [HttpPost("{id:int}/upload-image")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImage(int id, IFormFile image, CancellationToken ct)
    {
        var imageUrl = await _sender.Send(
            new UploadProblemReportImageCommand { ReportId = id, Image = image }, ct);
        return Ok(new { imageUrl });
    }

    [HttpGet("{id:int}/image")]
    public async Task<IActionResult> GetImage(int id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetProblemReportImageQuery { ReportId = id }, ct);
        return PhysicalFile(result.FilePath, result.MimeType);
    }

    [HttpPost("import")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ImportProblemReportsResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Import(
        [FromForm] ImportProblemReportsRequest request,
        CancellationToken ct = default)
    {
        var result = await _sender.Send(new ImportProblemReportsCommand
        {
            File = request.File,
            SkipFirstRow = request.SkipFirstRow,
            DryRun = request.DryRun
        }, ct);
        return Ok(result);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> PatchStatus(int id, [FromBody] PatchStatusRequest request, CancellationToken ct)
    {
        var command = new UpdateProblemReportCommand { Id = id, StatusId = request.StatusId };
        await _sender.Send(command, ct);
        return NoContent();
    }

    public record PatchStatusRequest(int StatusId);
}