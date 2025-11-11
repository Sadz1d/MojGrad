namespace Market.Application.Modules.Identity.Profiles.Queries.List;

public sealed class ListProfilesQueryDto
{
    public required int Id { get; init; }                // Id profila
    public required int UserId { get; init; }            // Id korisnika kojem profil pripada
    public string? Address { get; init; }               // Adresa
    public string? Phone { get; init; }                 // Telefon
    public string? ProfilePicture { get; init; }        // Slika profila (URL ili path)
    public string? BiographyText { get; init; }         // Biografija
    public required string UserFullName { get; init; }  // Ime i prezime korisnika
}
