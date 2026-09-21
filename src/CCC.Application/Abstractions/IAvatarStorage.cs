namespace CCC.Application.Abstractions;

/// <summary>
/// Uploads avatar images and downloads them back for the proxy endpoint.
/// Implemented in Infrastructure (Supabase Storage); consumed by the
/// Avatars vertical slice. Application code only knows "upload/download file".
/// </summary>
public interface IAvatarStorage
{
    Task<string> UploadAsync(
        Stream content,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the file bytes, or null when the file does not exist.
    /// </summary>
    Task<(byte[] Content, string ContentType)?> DownloadAsync(
        string fileName,
        CancellationToken cancellationToken = default);
}
