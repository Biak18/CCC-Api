using CCC.Application.Features.Avatars.GetAvatar;
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
    /// Uploads an avatar image and returns this API's URL for it.
    /// Store the returned <c>url</c> as <c>avatarUrl</c> when creating or
    /// updating a contact. Images are served through this API (not the
    /// Supabase domain) so they load in regions where it is blocked.
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

        // Canonical URL goes through this API, never the storage domain.
        var proxied = result with
        {
            Url = $"{PublicScheme()}://{Request.Host}/api/avatars/{result.FileName}"
        };

        return Created(proxied.Url, proxied);
    }

    /// <summary>
    /// Serves an avatar image proxied from storage.
    /// Filenames are unique per upload, so responses are cached immutably.
    /// </summary>
    [HttpGet("{fileName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Download(
        string fileName,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new GetAvatarQuery(fileName),
            cancellationToken);

        Response.Headers.CacheControl = "public,max-age=31536000,immutable";

        return File(result.Content, result.ContentType);
    }

    // Render terminates TLS at its proxy; honor the forwarded proto so
    // generated URLs use https in production and http locally.
    private string PublicScheme() =>
        Request.Headers["X-Forwarded-Proto"].FirstOrDefault()
        ?? Request.Scheme;
}
