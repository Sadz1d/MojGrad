namespace Market.Application.Modules.Identity.Profiles.Queries.GetById;

public sealed class GetProfileByIdQueryDto
{
    public required int Id { get; init; }
    public string? Address { get; init; }
    public string? Phone { get; init; }
    public string? ProfilePicture { get; init; }
    public string? BiographyText { get; init; }
    public string? UserFullName { get; init; }
}
