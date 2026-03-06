using MediatR;

namespace Market.Application.Modules.Identity.Profiles.Queries.GetPicture;

public sealed class GetProfilePictureQuery : IRequest<GetProfilePictureResult>
{
    public int UserId { get; init; }
}

public sealed class GetProfilePictureResult
{
    public string FilePath { get; init; } = string.Empty;
    public string MimeType { get; init; } = string.Empty;
}