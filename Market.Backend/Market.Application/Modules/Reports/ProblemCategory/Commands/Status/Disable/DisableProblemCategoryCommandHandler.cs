using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Market.Application.Modules.Reports.ProblemCategories.Commands.Status.Disable;

public sealed class DisableProblemCategoryCommandHandler(IAppDbContext ctx)
    : IRequestHandler<DisableProblemCategoryCommand, Unit>
{
    public async Task<Unit> Handle(DisableProblemCategoryCommand request, CancellationToken ct)
    {
        var category = await ctx.ProblemCategories
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (category is null)
            throw new MarketNotFoundException($"Problem category (ID={request.Id}) not found.");

        if (!category.IsEnabled)
            return Unit.Value; // idempotent

        // Business rule: cannot disable if there are reports
        var hasReports = await ctx.ProblemReports
            .AnyAsync(r => r.CategoryId == category.Id, ct);

        if (hasReports)
        {
            throw new MarketBusinessRuleException(
                "problemCategory.disable.blocked.reports",
                $"Category '{category.Name}' cannot be disabled because it has reports."
            );
        }

        category.IsEnabled = false;
        await ctx.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
