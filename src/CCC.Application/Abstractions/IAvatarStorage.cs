namespace CCC.Application.Abstractions;

/// <summary>
/// Uploads avatar images and returns a publicly accessible URL.
/// Implemented in Infrastructure (Supabase Storage); consumed by the
/// Avatars vertical slice. Application code only knows "upload file".
/// </summary>
public interface IAvatarStorage
{
    Task<string> UploadAsync(
        Stream content,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default);
}
