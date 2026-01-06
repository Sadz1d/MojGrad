// Market.Application.Modules.Reports.ProblemReport.Commands.Import.ImportProblemReportsCommandHandler.cs
using Market.Application.Abstractions;
using Market.Application.Modules.Reports.ProblemReport.Dtos;
using Market.Domain.Entities.Reports;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        var result = new ImportProblemReportsResult
        {
            TotalRecords = request.Items.Count
        };

        try
        {
            // Ako je dry run, samo validiraj
            if (request.DryRun)
            {
                await ValidateItems(request.Items, result, cancellationToken);
                return result;
            }

            // Inače, validiraj i snimi
            await ValidateAndImportItems(request.Items, result, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Greška pri importu prijava problema");
            throw;
        }

        return result;
    }

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
                // Proveri da li kategorija i status postoje u bazi
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

                // Validacija osnovnih podataka
                var errors = ValidateItem(item, rowNumber);
                if (errors.Any())
                {
                    result.Failed++;
                    result.Errors.AddRange(errors);
                    continue;
                }

                // Validacija u odnosu na bazu
                var dbErrors = await ValidateAgainstDatabase(item, rowNumber, cancellationToken);
                if (dbErrors.Any())
                {
                    result.Failed++;
                    result.Errors.AddRange(dbErrors);
                    continue;
                }

                try
                {
                    // Kreiraj novu prijavu
                    var problemReport = new ProblemReportEntity
                    {
                        Title = item.Title,
                        Description = item.Description,
                        Location = item.Location,
                        CategoryId = item.CategoryId,
                        StatusId = item.StatusId,
                        UserId = item.UserId ?? _currentUser.UserId ?? throw new UnauthorizedAccessException("Korisnik nije autentifikovan"),
                        CreationDate = DateTime.UtcNow,
                        
                    };

                    await _context.ProblemReports.AddAsync(problemReport, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);

                    result.Successful++;
                    result.ImportedIds.Add(problemReport.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Greška pri snimanju reda {rowNumber}");
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

    private List<string> ValidateItem(ImportProblemReportDto item, int rowNumber)
    {
        var errors = new List<string>();

        // Validacija Title
        if (string.IsNullOrWhiteSpace(item.Title))
            errors.Add($"Red {rowNumber}: Naslov je obavezno polje");
        else if (item.Title.Length < 5 || item.Title.Length > 200)
            errors.Add($"Red {rowNumber}: Naslov mora imati između 5 i 200 karaktera");

        // Validacija Description
        if (string.IsNullOrWhiteSpace(item.Description))
            errors.Add($"Red {rowNumber}: Opis je obavezno polje");
        else if (item.Description.Length < 10 || item.Description.Length > 2000)
            errors.Add($"Red {rowNumber}: Opis mora imati između 10 i 2000 karaktera");

        // Validacija CategoryId
        if (item.CategoryId <= 0)
            errors.Add($"Red {rowNumber}: ID kategorije mora biti pozitivan broj");

        // Validacija StatusId
        if (item.StatusId <= 0)
            errors.Add($"Red {rowNumber}: ID statusa mora biti pozitivan broj");

        // Validacija Location (opcionalno)
        if (!string.IsNullOrWhiteSpace(item.Location) && item.Location.Length > 200)
            errors.Add($"Red {rowNumber}: Lokacija ne može biti duža od 200 karaktera");

        // Validacija UserId (opcionalno)
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

        // Proveri da li kategorija postoji
        var categoryExists = await _context.ProblemCategories
            .AnyAsync(c => c.Id == item.CategoryId, cancellationToken);

        if (!categoryExists)
            errors.Add($"Red {rowNumber}: Kategorija sa ID {item.CategoryId} ne postoji");

        // Proveri da li status postoji
        var statusExists = await _context.ProblemStatuses
            .AnyAsync(s => s.Id == item.StatusId, cancellationToken);

        if (!statusExists)
            errors.Add($"Red {rowNumber}: Status sa ID {item.StatusId} ne postoji");

        // Opcionalno: proveri da li korisnik postoji
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