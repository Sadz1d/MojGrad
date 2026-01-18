using CsvHelper;
using Market.API.Models.Requests;
using Market.Application.Abstractions;
using Market.Application.Modules.Reports.ProblemReport.Commands.Create;
using Market.Application.Modules.Reports.ProblemReport.Commands.Delete;
using Market.Application.Modules.Reports.ProblemReport.Commands.Import;
using Market.Application.Modules.Reports.ProblemReport.Commands.Update;
using Market.Application.Modules.Reports.ProblemReport.Dtos;
using Market.Application.Modules.Reports.ProblemReport.Queries.GetById;
using Market.Application.Modules.Reports.ProblemReport.Queries.GetPaged;
using Market.Application.Modules.Reports.ProblemReport.Queries.List;
using Market.Domain.Entities.Reports;
using MediatR;
//using Market.Application.Modules.Reports.ProblemReport.Queries.GetById;
//using Market.Application.Modules.Reports.ProblemReport.Commands.Create;
//using Market.Application.Modules.Reports.ProblemReport.Commands.Update;
//using Market.Application.Modules.Reports.ProblemReport.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml; // Dodajte za Excel podršku
using System.Formats.Asn1;
using System.Globalization;
using System.Text.Json;

namespace Market.API.Controllers;

[ApiController]
[Route("api/reports/problem-reports")]
public sealed class ProblemReportsController : ControllerBase
{
    private readonly ISender sender;
    private readonly ILogger<ProblemReportsController> _logger;
    private readonly IAppDbContext _dbContext;
    public ProblemReportsController(ISender sender,
        ILogger<ProblemReportsController> logger, IAppDbContext dbContext)
    {
        this.sender = sender;
        _logger = logger;
        _dbContext = dbContext;
        // EPPlus 8+ license setting (replaces obsolete LicenseContext)
        OfficeOpenXml.ExcelPackage.License.SetNonCommercialPersonal("Your Name or Organization");
    }

    [HttpGet]
    public async Task<PageResult<ListProblemReportQueryDto>> List(
        [FromQuery] ListProblemReportQuery query, CancellationToken ct)
    {
        // Dodajte logging
        _logger.LogInformation("List called with Page={Page}, PageSize={PageSize}, Search={Search}",
            query.Page, query.PageSize, query.Search);

        return await sender.Send(query, ct);
    }


    [HttpGet("{id:int}")]
    public async Task<GetProblemReportByIdQueryDto> GetById(int id, CancellationToken ct)
    => await sender.Send(new GetProblemReportByIdQuery { Id = id }, ct);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProblemReportCommand command, CancellationToken ct)
    {
        var id = await sender.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProblemReportCommand command, CancellationToken ct)
    {
        command.Id = id;
        await sender.Send(command, ct);
        return NoContent();
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await sender.Send(new DeleteProblemReportCommand { Id = id }, ct);
        return NoContent();
    }
    [HttpPost("import")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ImportProblemReportsResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Import(
    [FromForm] ImportProblemReportsRequest request,
    CancellationToken cancellationToken)
    {
        try
        {
            // Validacija fajla
            if (request.File == null || request.File.Length == 0)
                return BadRequest(new { message = "Nije odabran fajl." });

            if (request.File.Length > 10 * 1024 * 1024) // 10MB limit
                return BadRequest(new { message = "Fajl je prevelik. Maksimalna veličina je 10MB." });

            var allowedExtensions = new[] { ".json", ".xlsx", ".xls", ".csv" };
            var fileExtension = Path.GetExtension(request.File.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
                return BadRequest(new
                {
                    message = "Nepodržan format fajla. Podržani formati: JSON, Excel (.xlsx, .xls), CSV (.csv)"
                });

            // Parsiranje fajla
            List<ImportProblemReportDto> items;

            if (fileExtension == ".json")
            {
                items = await ParseJsonFile(request.File, cancellationToken);
            }
            else if (fileExtension == ".csv")
            {
                items = await ParseCsvFile(request.File, request.SkipFirstRow, cancellationToken);
            }
            else // Excel (.xlsx, .xls)
            {
                items = await ParseExcelFile(request.File, request.SkipFirstRow, cancellationToken);
            }

            // Validacija podataka
            if (items == null || !items.Any())
                return BadRequest(new { message = "Fajl ne sadrži validne podatke." });

            // Pošalji command
            var command = new ImportProblemReportsCommand
            {
                Items = items,
                DryRun = request.DryRun
            };

            var result = await sender.Send(command, cancellationToken);

            return Ok(result);
        }
        catch (JsonException ex)
        {
            return BadRequest(new { message = "Nevalidan JSON format: " + ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Greška pri importu prijava problema");
            return StatusCode(500, new { message = "Došlo je do greške pri importu." });
        }
    }

    private async Task<List<ImportProblemReportDto>> ParseJsonFile(
        IFormFile file,
        CancellationToken cancellationToken)
    {
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream, cancellationToken);
        stream.Position = 0;

        var json = await new StreamReader(stream).ReadToEndAsync();

        return JsonSerializer.Deserialize<List<ImportProblemReportDto>>(
            json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        ) ?? new List<ImportProblemReportDto>();
    }

    private async Task<List<ImportProblemReportDto>> ParseCsvFile(
        IFormFile file,
        bool skipFirstRow,
        CancellationToken cancellationToken)
    {
        var items = new List<ImportProblemReportDto>();

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream, cancellationToken);
        stream.Position = 0;

        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<ImportProblemReportDtoMap>();

        if (skipFirstRow)
            csv.Read();

        await foreach (var record in csv.GetRecordsAsync<ImportProblemReportDto>(cancellationToken))
        {
            items.Add(record);
        }

        return items;
    }

    private async Task<List<ImportProblemReportDto>> ParseExcelFile(
        IFormFile file,
        bool skipFirstRow,
        CancellationToken cancellationToken)
    {
       
        var items = new List<ImportProblemReportDto>();

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream, cancellationToken);
        stream.Position = 0;

        using var package = new ExcelPackage(stream);
        var worksheet = package.Workbook.Worksheets[0];

        var startRow = skipFirstRow ? 2 : 1;
        var endRow = worksheet.Dimension?.End.Row ?? 0;

        for (int row = startRow; row <= endRow; row++)
        {
            // Pokušaj parsiranje reda
            var item = ParseExcelRow(worksheet, row);
            if (item != null)
                items.Add(item);
        }

        return items;
    }

    private ImportProblemReportDto? ParseExcelRow(ExcelWorksheet worksheet, int row)
    {
        try
        {
            var title = worksheet.Cells[row, 1].Text?.Trim();
            var description = worksheet.Cells[row, 2].Text?.Trim();
            var location = worksheet.Cells[row, 3].Text?.Trim();

            // Preskoči prazne redove
            if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(description))
                return null;

            return new ImportProblemReportDto
            {
                Title = title ?? string.Empty,
                Description = description ?? string.Empty,
                Location = location,
                CategoryId = int.TryParse(worksheet.Cells[row, 4].Text, out var catId) ? catId : 0,
                StatusId = int.TryParse(worksheet.Cells[row, 5].Text, out var statusId) ? statusId : 0,
                UserId = int.TryParse(worksheet.Cells[row, 6].Text, out var userId) ? userId : null
            };
        }
        catch
        {
            // Preskoči redove sa greškama
            return null;
        }
    }


    //private readonly IMediator _mediator;

    //public ProblemReportsController(IMediator mediator)
    //{
    //    _mediator = mediator;
    //}

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged(
[FromQuery] GetPagedProblemReportsQuery query, // Ovdje promijenite
CancellationToken ct = default)
    {
        var result = await sender.Send(query, ct);
        return Ok(result);
    }


    [HttpPost("{id:int}/upload-image")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImage(
    int id,
    IFormFile image,
    CancellationToken ct)
    {
        if (image == null || image.Length == 0)
            return BadRequest("Slika nije odabrana.");

        var report = await sender.Send(
            new GetProblemReportByIdQuery { Id = id }, ct);

        if (report == null)
            return NotFound();

        var uploadsRoot = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Uploads",
            "ProblemReports");

        if (!Directory.Exists(uploadsRoot))
            Directory.CreateDirectory(uploadsRoot);

        var extension = Path.GetExtension(image.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsRoot, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await image.CopyToAsync(stream, ct);

        // UPDATE entity (preko command-a ili repo-a)
        await sender.Send(new UpdateProblemReportImageCommand
        {
            ProblemReportId = id,
            ImagePath = $"/Uploads/ProblemReports/{fileName}"
        }, ct);

        return Ok(new { imageUrl = $"/Uploads/ProblemReports/{fileName}" });
    }

    [Authorize] // zahtijeva prijavljeni korisnik
    [HttpGet("{id}/image")]
    public IActionResult GetReportImage(int id)
    {
        var report = _dbContext.ProblemReports.Find(id);
        if (report == null || string.IsNullOrEmpty(report.ImagePath))
            return NotFound();

        var path = Path.Combine(Directory.GetCurrentDirectory(), report.ImagePath);
        if (!System.IO.File.Exists(path))
            return NotFound();

        // Odredi MIME tip po ekstenziji
        var ext = Path.GetExtension(path).ToLower();
        var mime = ext switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            _ => "application/octet-stream"
        };

        return PhysicalFile(path, mime);
    }

}