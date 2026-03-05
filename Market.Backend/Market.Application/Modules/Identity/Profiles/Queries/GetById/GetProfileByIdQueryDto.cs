namespace Market.Application.Modules.Identity.Profiles.Queries.GetById;

public sealed class GetProfileByIdQueryDto
{
    public required int Id { get; init; }
    public string? Address { get; init; }
    public string? Phone { get; init; }
    public string? ProfilePicture { get; init; }
    public string? BiographyText { get; init; }
    public string? UserFullName { get; init; }

    // Extra user fields
    public int UserId { get; init; }
    public string? Email { get; init; }
    public int Points { get; init; }
    public DateTime RegistrationDate { get; init; }
    public bool IsAdmin { get; init; }
    public bool IsManager { get; init; }
    public bool IsEmployee { get; init; }
    public int ReportsCount { get; init; }
}