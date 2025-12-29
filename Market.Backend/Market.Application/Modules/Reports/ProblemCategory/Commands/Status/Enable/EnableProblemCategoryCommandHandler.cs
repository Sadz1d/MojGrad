using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Market.Application.Modules.Reports.ProblemCategories.Commands.Status.Enable;

public sealed class EnableProblemCategoryCommandHandler(IAppDbContext ctx)
    : IRequestHandler<EnableProblemCategoryCommand, Unit>
{
    public async Task<Unit> Handle(EnableProblemCategoryCommand request, CancellationToken ct)
    {
        var category = await ctx.ProblemCategories
            .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        if (category is null)
            throw new MarketNotFoundException($"Problem category (ID={request.Id}) not found.");

        if (!category.IsEnabled)
        {
            category.IsEnabled = true;
            await ctx.SaveChangesAsync(ct);
        }

        return Unit.Value;
    }
}
