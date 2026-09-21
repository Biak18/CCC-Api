using CCC.Application.Features.Avatars.UploadAvatar;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CCC.Api.Controllers;

[ApiController]
[Route("api/avatars")]
public sealed class AvatarsController(ISender sender) : ControllerBase
{
    private const long MaxBytes = 5 * 1024 * 1024;

    /// <summary>
    /// Uploads an avatar image to Supabase Storage and returns its public URL.
    /// Store the returned <c>url</c> as <c>avatarUrl</c> when creating or
    /// updating a contact.
    /// </summary>
    [HttpPost]
    [RequestSizeLimit(MaxBytes + (512 * 1024))]
    [ProducesResponseType(typeof(UploadAvatarResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UploadAvatarResponse>> Upload(
        IFormFile file,
        CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest("A file must be provided.");
        }

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream, cancellationToken);
        stream.Position = 0;

        var command = new UploadAvatarCommand(
            stream,
            file.FileName,
            string.IsNullOrWhiteSpace(file.ContentType)
                ? "application/octet-stream"
                : file.ContentType,
            file.Length);

        var result = await sender.Send(command, cancellationToken);

        return Created(result.Url, result);
    }
}
