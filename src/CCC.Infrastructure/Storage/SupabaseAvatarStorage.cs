using CCC.Application.Abstractions;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace CCC.Infrastructure.Storage;

/// <summary>
/// Stores avatars in the Supabase Storage "avatars" bucket via the
/// Storage REST API and returns the public URL. Keeps existing public
/// URLs working - clients store the returned URL on the contact.
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
}
