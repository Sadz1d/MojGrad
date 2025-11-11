namespace Market.Application.Modules.Identity.Users.Queries.List;

public sealed class ListMarketUsersQueryDto
{
    public required int Id { get; init; }
    public required string Email { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public required bool IsAdmin { get; init; }
    public required bool IsManager { get; init; }
    public required bool IsEmployee { get; init; }
    public required bool IsEnabled { get; init; }
    public required DateTime RegistrationDate { get; init; }
}
