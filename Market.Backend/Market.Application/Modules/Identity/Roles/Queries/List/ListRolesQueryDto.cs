namespace Market.Application.Modules.Identity.Roles.Queries.List;

public sealed class ListRolesQueryDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
}
