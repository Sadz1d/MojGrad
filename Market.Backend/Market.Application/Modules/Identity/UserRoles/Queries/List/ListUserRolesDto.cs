namespace Market.Application.Modules.Identity.UserRoles.Queries.List;

public sealed class ListUserRolesDto
{
    public required int Id { get; init; }
    public required int UserId { get; init; }
    public required int RoleId { get; init; }
    public string? UserName { get; init; }
    public string? RoleName { get; init; }
}
