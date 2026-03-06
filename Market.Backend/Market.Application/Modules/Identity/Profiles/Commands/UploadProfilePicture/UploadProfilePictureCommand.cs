using MediatR;
using Microsoft.AspNetCore.Http;

namespace Market.Application.Modules.Identity.Profiles.Commands.UploadPicture;

public sealed class UploadProfilePictureCommand : IRequest<string>
{
    public int UserId { get; init; }
    public IFormFile Image { get; init; } = null!;
}