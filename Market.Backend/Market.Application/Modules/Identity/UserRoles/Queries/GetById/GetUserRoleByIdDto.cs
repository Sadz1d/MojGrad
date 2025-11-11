namespace Market.Application.Modules.Identity.UserRoles.Queries.GetById;

public sealed class GetUserRoleByIdDto
{
    public required int Id { get; init; }
    public required int UserId { get; init; }
    public required int RoleId { get; init; }
    public string? UserName { get; init; }
    public string? RoleName { get; init; }
}
