namespace Market.Application.Modules.Identity.Users.Queries.GetById;

public sealed class GetMarketUserByIdQueryDto
{
    public required int Id { get; init; }
    public required string Email { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public bool IsAdmin { get; init; }
    public bool IsManager { get; init; }
    public bool IsEmployee { get; init; }
    public bool IsEnabled { get; init; }
    public DateTime RegistrationDate { get; init; }
}
