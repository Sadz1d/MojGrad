// Market.Application.Modules.Reports.ProblemReport.Commands.Import.ImportProblemReportsCommandHandler.cs
using CsvHelper;
using Market.Application.Abstractions;
using Market.Application.Common.Exceptions;
using Market.Application.Modules.Reports.ProblemReport.Dtos;
using Market.Domain.Entities.Reports;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System.Globalization;
using System.Text.Json;

namespace Market.Application.Modules.Reports.ProblemReport.Commands.Import;

public class ImportProblemReportsCommandHandler : IRequestHandler<ImportProblemReportsCommand, ImportProblemReportsResult>
{
    private readonly IAppDbContext _context;
    private readonly ILogger<ImportProblemReportsCommandHandler> _logger;
    private readonly IAppCurrentUser _currentUser;

    public ImportProblemReportsCommandHandler(
        IAppDbContext context,
        ILogger<ImportProblemReportsCommandHandler> logger,
        IAppCurrentUser currentUser)
    {
        _context = context;
        _logger = logger;
        _currentUser = currentUser;
    }

    public async Task<ImportProblemReportsResult> Handle(
        ImportProblemReportsCommand request,
        CancellationToken cancellationToken)
    {
        // Parse file if provided, otherwise use pre-parsed items
        List<ImportProblemReportDto> items;

        if (request.File != null)
        {
            ValidateFile(request.File);
            items = await ParseFile(request.File, request.SkipFirstRow, cancellationToken);
        }
        else if (request.Items != null && request.Items.Any())
        {
            items = request.Items;
        }
        else
        {
            throw new MarketNotFoundException("Nije proslijeđen fajl niti lista stavki za import.");
        }

        if (!items.Any())
            throw new MarketNotFoundException("Fajl ne sadrži validne podatke.");

        var result = new ImportProblemReportsResult { TotalRecords = items.Count };

        try
        {
            if (request.DryRun)
            {
                await ValidateItems(items, result, cancellationToken);
            }
            else
            {
                await ValidateAndImportItems(items, result, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Greška pri importu prijava problema");
            throw;
        }

        return result;
    }

    // ── File parsing ─────────────────────────────────────────────────────────

    private static void ValidateFile(Microsoft.AspNetCore.Http.IFormFile file)
    {
        if (file.Length == 0)
            throw new MarketNotFoundException("Nije odabran fajl.");

        if (file.Length > 10 * 1024 * 1024)
            throw new MarketNotFoundException("Fajl je prevelik. Maksimalna veličina je 10MB.");

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        var allowed = new[] { ".json", ".xlsx", ".xls", ".csv" };
        if (!allowed.Contains(ext))
            throw new MarketNotFoundException(
                "Nepodržan format fajla. Podržani formati: JSON, Excel (.xlsx, .xls), CSV (.csv)");
    }

    private static async Task<List<ImportProblemReportDto>> ParseFile(
        Microsoft.AspNetCore.Http.IFormFile file,
        bool skipFirstRow,
        CancellationToken ct)
    {
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

        return ext switch
        {
            ".json" => await ParseJson(file, ct),
            ".csv" => await ParseCsv(file, skipFirstRow, ct),
            ".xlsx" or ".xls" => await ParseExcel(file, skipFirstRow),
            _ => throw new MarketNotFoundException("Nepodržan format fajla.")
        };
    }

    private static async Task<List<ImportProblemReportDto>> ParseJson(
        Microsoft.AspNetCore.Http.IFormFile file, CancellationToken ct)
    {
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream, ct);
        stream.Position = 0;
        var json = await new StreamReader(stream).ReadToEndAsync(ct);
        return JsonSerializer.Deserialize<List<ImportProblemReportDto>>(
            json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? [];
    }

    private static async Task<List<ImportProblemReportDto>> ParseCsv(
        Microsoft.AspNetCore.Http.IFormFile file, bool skipFirstRow, CancellationToken ct)
    {
        var items = new List<ImportProblemReportDto>();
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream, ct);
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<ImportProblemReportDtoMap>();
        if (skipFirstRow) csv.Read();
        await foreach (var record in csv.GetRecordsAsync<ImportProblemReportDto>(ct))
            items.Add(record);
        return items;
    }

    private static async Task<List<ImportProblemReportDto>> ParseExcel(
        Microsoft.AspNetCore.Http.IFormFile file, bool skipFirstRow)
    {
        ExcelPackage.License.SetNonCommercialPersonal("Moj Grad");
        var items = new List<ImportProblemReportDto>();
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        stream.Position = 0;
        using var package = new ExcelPackage(stream);
        var ws = package.Workbook.Worksheets[0];
        var startRow = skipFirstRow ? 2 : 1;
        var endRow = ws.Dimension?.End.Row ?? 0;
        for (int row = startRow; row <= endRow; row++)
        {
            var item = ParseExcelRow(ws, row);
            if (item != null) items.Add(item);
        }
        return items;
    }

    private static ImportProblemReportDto? ParseExcelRow(ExcelWorksheet ws, int row)
    {
        try
        {
            var title = ws.Cells[row, 1].Text?.Trim();
            var description = ws.Cells[row, 2].Text?.Trim();
            if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(description))
                return null;

            return new ImportProblemReportDto
            {
                Title = title ?? string.Empty,
                Description = description ?? string.Empty,
                Location = ws.Cells[row, 3].Text?.Trim(),
                CategoryId = int.TryParse(ws.Cells[row, 4].Text, out var cat) ? cat : 0,
                StatusId = int.TryParse(ws.Cells[row, 5].Text, out var st) ? st : 0,
                UserId = int.TryParse(ws.Cells[row, 6].Text, out var uid) ? uid : null
            };
        }
        catch { return null; }
    }

    // ── Validation & import (unchanged) ──────────────────────────────────────

    private async Task ValidateItems(
        List<ImportProblemReportDto> items,
        ImportProblemReportsResult result,
        CancellationToken cancellationToken)
    {
        foreach (var (item, index) in items.Select((item, index) => (item, index)))
        {
            var rowNumber = index + 1;
            var errors = ValidateItem(item, rowNumber);

            if (errors.Any())
            {
                result.Failed++;
                result.Errors.AddRange(errors);
            }
            else
            {
                var validationErrors = await ValidateAgainstDatabase(item, rowNumber, cancellationToken);
                if (validationErrors.Any())
                {
                    result.Failed++;
                    result.Errors.AddRange(validationErrors);
                }
                else
                {
                    result.Successful++;
                }
            }
        }
    }

    private async Task ValidateAndImportItems(
        List<ImportProblemReportDto> items,
        ImportProblemReportsResult result,
        CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            foreach (var (item, index) in items.Select((item, index) => (item, index)))
            {
                var rowNumber = index + 1;

                var errors = ValidateItem(item, rowNumber);
                if (errors.Any())
                {
                    result.Failed++;
                    result.Errors.AddRange(errors);
                    continue;
                }

                var dbErrors = await ValidateAgainstDatabase(item, rowNumber, cancellationToken);
                if (dbErrors.Any())
                {
                    result.Failed++;
                    result.Errors.AddRange(dbErrors);
                    continue;
                }

                try
                {
                    var problemReport = new ProblemReportEntity
                    {
                        Title = item.Title,
                        Description = item.Description,
                        Location = item.Location,
                        CategoryId = item.CategoryId,
                        StatusId = item.StatusId,
                        UserId = item.UserId ?? _currentUser.UserId
                            ?? throw new UnauthorizedAccessException("Korisnik nije autentifikovan"),
                        CreationDate = DateTime.UtcNow,
                    };

                    await _context.ProblemReports.AddAsync(problemReport, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);

                    result.Successful++;
                    result.ImportedIds.Add(problemReport.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Greška pri snimanju reda {Row}", rowNumber);
                    result.Failed++;
                    result.Errors.Add($"Red {rowNumber}: Greška pri snimanju - {ex.Message}");
                }
            }

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Greška pri importu - rollback transakcije");
            throw;
        }
    }

    private static List<string> ValidateItem(ImportProblemReportDto item, int rowNumber)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(item.Title))
            errors.Add($"Red {rowNumber}: Naslov je obavezno polje");
        else if (item.Title.Length < 5 || item.Title.Length > 200)
            errors.Add($"Red {rowNumber}: Naslov mora imati između 5 i 200 karaktera");

        if (string.IsNullOrWhiteSpace(item.Description))
            errors.Add($"Red {rowNumber}: Opis je obavezno polje");
        else if (item.Description.Length < 10 || item.Description.Length > 2000)
            errors.Add($"Red {rowNumber}: Opis mora imati između 10 i 2000 karaktera");

        if (item.CategoryId <= 0)
            errors.Add($"Red {rowNumber}: ID kategorije mora biti pozitivan broj");

        if (item.StatusId <= 0)
            errors.Add($"Red {rowNumber}: ID statusa mora biti pozitivan broj");

        if (!string.IsNullOrWhiteSpace(item.Location) && item.Location.Length > 200)
            errors.Add($"Red {rowNumber}: Lokacija ne može biti duža od 200 karaktera");

        if (item.UserId.HasValue && item.UserId <= 0)
            errors.Add($"Red {rowNumber}: ID korisnika mora biti pozitivan broj");

        return errors;
    }

    private async Task<List<string>> ValidateAgainstDatabase(
        ImportProblemReportDto item,
        int rowNumber,
        CancellationToken cancellationToken)
    {
        var errors = new List<string>();

        var categoryExists = await _context.ProblemCategories
            .AnyAsync(c => c.Id == item.CategoryId, cancellationToken);
        if (!categoryExists)
            errors.Add($"Red {rowNumber}: Kategorija sa ID {item.CategoryId} ne postoji");

        var statusExists = await _context.ProblemStatuses
            .AnyAsync(s => s.Id == item.StatusId, cancellationToken);
        if (!statusExists)
            errors.Add($"Red {rowNumber}: Status sa ID {item.StatusId} ne postoji");

        if (item.UserId.HasValue)
        {
            var userExists = await _context.Users
                .AnyAsync(u => u.Id == item.UserId.Value, cancellationToken);
            if (!userExists)
                errors.Add($"Red {rowNumber}: Korisnik sa ID {item.UserId.Value} ne postoji");
        }

        return errors;
    }
}