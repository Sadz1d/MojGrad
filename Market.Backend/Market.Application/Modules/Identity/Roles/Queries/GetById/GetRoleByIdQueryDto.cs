namespace Market.Application.Modules.Identity.Roles.Queries.GetById;

public sealed class GetRoleByIdQueryDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
}
