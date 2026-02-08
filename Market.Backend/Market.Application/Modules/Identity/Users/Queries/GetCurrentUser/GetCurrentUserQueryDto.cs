namespace Market.Application.Modules.Identity.Users.Queries.GetCurrentUser;

public sealed class GetCurrentUserQueryDto
{
    public required int Id { get; init; }
    public required string Email { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? FullName { get; init; }

    public bool IsAdmin { get; init; }
    public bool IsManager { get; init; }
    public bool IsEmployee { get; init; }
}
