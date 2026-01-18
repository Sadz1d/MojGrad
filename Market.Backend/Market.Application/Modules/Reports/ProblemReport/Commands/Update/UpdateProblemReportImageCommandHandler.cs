using MediatR;


namespace Market.Application.Modules.Reports.ProblemReport.Commands.Update;

public sealed class UpdateProblemReportImageCommandHandler
    : IRequestHandler<UpdateProblemReportImageCommand>
{
    private readonly IAppDbContext _context;

    public UpdateProblemReportImageCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateProblemReportImageCommand request, CancellationToken ct)
    {
        var report = await _context.ProblemReports
            .FindAsync(new object[] { request.ProblemReportId }, ct);

        if (report == null)
            throw new Exception("Problem report not found");

        // JEDNA SLIKA → overwrite
        report.ImagePath = request.ImagePath;

        await _context.SaveChangesAsync(ct);
    }
}
