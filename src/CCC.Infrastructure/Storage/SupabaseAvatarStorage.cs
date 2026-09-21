using CCC.Application.Abstractions;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace CCC.Infrastructure.Storage;

/// <summary>
/// Stores avatars in the Supabase Storage "avatars" bucket via the
/// Storage REST API. Clients never talk to Supabase directly - they use
/// this API's /api/avatars endpoints, so images load even where the
/// Supabase domain is blocked.
/// </summary>
public sealed class SupabaseAvatarStorage(
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration) : IAvatarStorage
{
    private const string Bucket = "avatars";

    public async Task<string> UploadAsync(
        Stream content,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        var supabaseUrl = configuration["Supabase:Url"]?.TrimEnd('/')
            ?? throw new InvalidOperationException("Supabase:Url is required.");

        var supabaseAnonKey = configuration["Supabase:AnonKey"]
            ?? throw new InvalidOperationException("Supabase:AnonKey is required.");

        var client = httpClientFactory.CreateClient("supabase-storage");

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"{supabaseUrl}/storage/v1/object/{Bucket}/{fileName}");

        request.Headers.Add("apikey", supabaseAnonKey);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", supabaseAnonKey);
        request.Headers.Add("x-upsert", "true");

        using var streamContent = new StreamContent(content);
        streamContent.Headers.ContentType =
            MediaTypeHeaderValue.Parse(contentType);
        request.Content = streamContent;

        using var response = await client.SendAsync(
            request,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException(
                $"Avatar upload failed ({(int)response.StatusCode}): {body}");
        }

        return $"{supabaseUrl}/storage/v1/object/public/{Bucket}/{fileName}";
    }

    public async Task<(byte[] Content, string ContentType)?> DownloadAsync(
        string fileName,
        CancellationToken cancellationToken = default)
    {
        var supabaseUrl = configuration["Supabase:Url"]?.TrimEnd('/')
            ?? throw new InvalidOperationException("Supabase:Url is required.");

        var supabaseAnonKey = configuration["Supabase:AnonKey"]
            ?? throw new InvalidOperationException("Supabase:AnonKey is required.");

        var client = httpClientFactory.CreateClient("supabase-storage");

        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{supabaseUrl}/storage/v1/object/{Bucket}/{fileName}");

        request.Headers.Add("apikey", supabaseAnonKey);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", supabaseAnonKey);

        using var response = await client.SendAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException(
                $"Avatar download failed ({(int)response.StatusCode}): {body}");
        }

        var content = await response.Content.ReadAsByteArrayAsync(cancellationToken);
        var contentType = response.Content.Headers.ContentType?.MediaType
            ?? "application/octet-stream";

        return (content, contentType);
    }
}
