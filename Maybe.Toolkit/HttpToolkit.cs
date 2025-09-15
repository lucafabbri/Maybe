using System.Net;
using Maybe;

namespace Maybe.Toolkit;

/// <summary>
/// Provides Maybe-based wrapper methods for HttpClient operations.
/// </summary>
public static class HttpToolkit
{
    /// <summary>
    /// Attempts to send a GET request to the specified Uri, returning a Maybe result.
    /// </summary>
    /// <param name="client">The HttpClient to use for the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <returns>A Maybe containing the HttpResponseMessage or an HttpError.</returns>
    public static async Task<Maybe<HttpResponseMessage, HttpError>> TryGetAsync(this HttpClient client, string? requestUri)
    {
        if (client == null)
        {
            return new HttpError(new ArgumentNullException(nameof(client)), requestUri, null, "HttpClient cannot be null");
        }

        if (string.IsNullOrWhiteSpace(requestUri))
        {
            return new HttpError(new ArgumentException("Request URI cannot be null or empty"), requestUri, null, "Request URI cannot be null or empty");
        }

        try
        {
            var response = await client.GetAsync(requestUri).ConfigureAwait(false);
            return Maybe<HttpResponseMessage, HttpError>.Some(response);
        }
        catch (HttpRequestException ex)
        {
            return new HttpError(ex, requestUri, null, $"HTTP request failed for URI: {requestUri}");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new HttpError(ex, requestUri, null, $"HTTP request timed out for URI: {requestUri}");
        }
        catch (TaskCanceledException ex)
        {
            return new HttpError(ex, requestUri, null, $"HTTP request was cancelled for URI: {requestUri}");
        }
        catch (Exception ex)
        {
            return new HttpError(ex, requestUri, null, $"Unexpected error during HTTP GET request to URI: {requestUri}");
        }
    }

    /// <summary>
    /// Attempts to send a GET request to the specified Uri, returning a Maybe result.
    /// </summary>
    /// <param name="client">The HttpClient to use for the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <returns>A Maybe containing the HttpResponseMessage or an HttpError.</returns>
    public static async Task<Maybe<HttpResponseMessage, HttpError>> TryGetAsync(this HttpClient client, Uri? requestUri)
    {
        if (client == null)
        {
            return new HttpError(new ArgumentNullException(nameof(client)), requestUri?.ToString(), null, "HttpClient cannot be null");
        }

        if (requestUri == null)
        {
            return new HttpError(new ArgumentNullException(nameof(requestUri)), null, null, "Request URI cannot be null");
        }

        try
        {
            var response = await client.GetAsync(requestUri).ConfigureAwait(false);
            return Maybe<HttpResponseMessage, HttpError>.Some(response);
        }
        catch (HttpRequestException ex)
        {
            return new HttpError(ex, requestUri.ToString(), null, $"HTTP request failed for URI: {requestUri}");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new HttpError(ex, requestUri.ToString(), null, $"HTTP request timed out for URI: {requestUri}");
        }
        catch (TaskCanceledException ex)
        {
            return new HttpError(ex, requestUri.ToString(), null, $"HTTP request was cancelled for URI: {requestUri}");
        }
        catch (Exception ex)
        {
            return new HttpError(ex, requestUri.ToString(), null, $"Unexpected error during HTTP GET request to URI: {requestUri}");
        }
    }

    /// <summary>
    /// Attempts to send a POST request to the specified Uri with the given content, returning a Maybe result.
    /// </summary>
    /// <param name="client">The HttpClient to use for the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <returns>A Maybe containing the HttpResponseMessage or an HttpError.</returns>
    public static async Task<Maybe<HttpResponseMessage, HttpError>> TryPostAsync(this HttpClient client, string? requestUri, HttpContent? content)
    {
        if (client == null)
        {
            return new HttpError(new ArgumentNullException(nameof(client)), requestUri, null, "HttpClient cannot be null");
        }

        if (string.IsNullOrWhiteSpace(requestUri))
        {
            return new HttpError(new ArgumentException("Request URI cannot be null or empty"), requestUri, null, "Request URI cannot be null or empty");
        }

        try
        {
            var response = await client.PostAsync(requestUri, content).ConfigureAwait(false);
            return Maybe<HttpResponseMessage, HttpError>.Some(response);
        }
        catch (HttpRequestException ex)
        {
            return new HttpError(ex, requestUri, null, $"HTTP POST request failed for URI: {requestUri}");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new HttpError(ex, requestUri, null, $"HTTP POST request timed out for URI: {requestUri}");
        }
        catch (TaskCanceledException ex)
        {
            return new HttpError(ex, requestUri, null, $"HTTP POST request was cancelled for URI: {requestUri}");
        }
        catch (Exception ex)
        {
            return new HttpError(ex, requestUri, null, $"Unexpected error during HTTP POST request to URI: {requestUri}");
        }
    }

    /// <summary>
    /// Attempts to send a GET request and read the response content as string, returning a Maybe result.
    /// </summary>
    /// <param name="client">The HttpClient to use for the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <returns>A Maybe containing the response content as string or an HttpError.</returns>
    public static async Task<Maybe<string, HttpError>> TryGetStringAsync(this HttpClient client, string? requestUri)
    {
        var responseResult = await client.TryGetAsync(requestUri).ConfigureAwait(false);
        
        return await responseResult.SelectAsync(async response =>
        {
            try
            {
                using (response)
                {
                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"Failed to read response content as string from URI: {requestUri}", ex);
            }
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Attempts to send a GET request and read the response content as byte array, returning a Maybe result.
    /// </summary>
    /// <param name="client">The HttpClient to use for the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <returns>A Maybe containing the response content as byte array or an HttpError.</returns>
    public static async Task<Maybe<byte[], HttpError>> TryGetByteArrayAsync(this HttpClient client, string? requestUri)
    {
        var responseResult = await client.TryGetAsync(requestUri).ConfigureAwait(false);
        
        return await responseResult.SelectAsync(async response =>
        {
            try
            {
                using (response)
                {
                    return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"Failed to read response content as byte array from URI: {requestUri}", ex);
            }
        }).ConfigureAwait(false);
    }
}