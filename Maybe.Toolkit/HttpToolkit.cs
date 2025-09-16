using System.Net;
using System.Text;
using System.Text.Json;
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
    /// Attempts to send a POST request to the specified Uri with the given content, returning a Maybe result.
    /// </summary>
    /// <param name="client">The HttpClient to use for the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <returns>A Maybe containing the HttpResponseMessage or an HttpError.</returns>
    public static async Task<Maybe<HttpResponseMessage, HttpError>> TryPostAsync(this HttpClient client, Uri? requestUri, HttpContent? content)
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
            var response = await client.PostAsync(requestUri, content).ConfigureAwait(false);
            return Maybe<HttpResponseMessage, HttpError>.Some(response);
        }
        catch (HttpRequestException ex)
        {
            return new HttpError(ex, requestUri.ToString(), null, $"HTTP POST request failed for URI: {requestUri}");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new HttpError(ex, requestUri.ToString(), null, $"HTTP POST request timed out for URI: {requestUri}");
        }
        catch (TaskCanceledException ex)
        {
            return new HttpError(ex, requestUri.ToString(), null, $"HTTP POST request was cancelled for URI: {requestUri}");
        }
        catch (Exception ex)
        {
            return new HttpError(ex, requestUri.ToString(), null, $"Unexpected error during HTTP POST request to URI: {requestUri}");
        }
    }

    /// <summary>
    /// Attempts to send a PUT request to the specified Uri with the given content, returning a Maybe result.
    /// </summary>
    /// <param name="client">The HttpClient to use for the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <returns>A Maybe containing the HttpResponseMessage or an HttpError.</returns>
    public static async Task<Maybe<HttpResponseMessage, HttpError>> TryPutAsync(this HttpClient client, string? requestUri, HttpContent? content)
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
            var response = await client.PutAsync(requestUri, content).ConfigureAwait(false);
            return Maybe<HttpResponseMessage, HttpError>.Some(response);
        }
        catch (HttpRequestException ex)
        {
            return new HttpError(ex, requestUri, null, $"HTTP PUT request failed for URI: {requestUri}");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new HttpError(ex, requestUri, null, $"HTTP PUT request timed out for URI: {requestUri}");
        }
        catch (TaskCanceledException ex)
        {
            return new HttpError(ex, requestUri, null, $"HTTP PUT request was cancelled for URI: {requestUri}");
        }
        catch (Exception ex)
        {
            return new HttpError(ex, requestUri, null, $"Unexpected error during HTTP PUT request to URI: {requestUri}");
        }
    }

    /// <summary>
    /// Attempts to send a PUT request to the specified Uri with the given content, returning a Maybe result.
    /// </summary>
    /// <param name="client">The HttpClient to use for the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <returns>A Maybe containing the HttpResponseMessage or an HttpError.</returns>
    public static async Task<Maybe<HttpResponseMessage, HttpError>> TryPutAsync(this HttpClient client, Uri? requestUri, HttpContent? content)
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
            var response = await client.PutAsync(requestUri, content).ConfigureAwait(false);
            return Maybe<HttpResponseMessage, HttpError>.Some(response);
        }
        catch (HttpRequestException ex)
        {
            return new HttpError(ex, requestUri.ToString(), null, $"HTTP PUT request failed for URI: {requestUri}");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new HttpError(ex, requestUri.ToString(), null, $"HTTP PUT request timed out for URI: {requestUri}");
        }
        catch (TaskCanceledException ex)
        {
            return new HttpError(ex, requestUri.ToString(), null, $"HTTP PUT request was cancelled for URI: {requestUri}");
        }
        catch (Exception ex)
        {
            return new HttpError(ex, requestUri.ToString(), null, $"Unexpected error during HTTP PUT request to URI: {requestUri}");
        }
    }

    /// <summary>
    /// Attempts to send a PATCH request to the specified Uri with the given content, returning a Maybe result.
    /// </summary>
    /// <param name="client">The HttpClient to use for the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <returns>A Maybe containing the HttpResponseMessage or an HttpError.</returns>
    public static async Task<Maybe<HttpResponseMessage, HttpError>> TryPatchAsync(this HttpClient client, string? requestUri, HttpContent? content)
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
#if NET8_0
            var response = await client.PatchAsync(requestUri, content).ConfigureAwait(false);
#else
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri)
            {
                Content = content
            };
            var response = await client.SendAsync(request).ConfigureAwait(false);
#endif
            return Maybe<HttpResponseMessage, HttpError>.Some(response);
        }
        catch (HttpRequestException ex)
        {
            return new HttpError(ex, requestUri, null, $"HTTP PATCH request failed for URI: {requestUri}");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new HttpError(ex, requestUri, null, $"HTTP PATCH request timed out for URI: {requestUri}");
        }
        catch (TaskCanceledException ex)
        {
            return new HttpError(ex, requestUri, null, $"HTTP PATCH request was cancelled for URI: {requestUri}");
        }
        catch (Exception ex)
        {
            return new HttpError(ex, requestUri, null, $"Unexpected error during HTTP PATCH request to URI: {requestUri}");
        }
    }

    /// <summary>
    /// Attempts to send a PATCH request to the specified Uri with the given content, returning a Maybe result.
    /// </summary>
    /// <param name="client">The HttpClient to use for the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="content">The HTTP request content sent to the server.</param>
    /// <returns>A Maybe containing the HttpResponseMessage or an HttpError.</returns>
    public static async Task<Maybe<HttpResponseMessage, HttpError>> TryPatchAsync(this HttpClient client, Uri? requestUri, HttpContent? content)
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
#if NET8_0
            var response = await client.PatchAsync(requestUri, content).ConfigureAwait(false);
#else
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri)
            {
                Content = content
            };
            var response = await client.SendAsync(request).ConfigureAwait(false);
#endif
            return Maybe<HttpResponseMessage, HttpError>.Some(response);
        }
        catch (HttpRequestException ex)
        {
            return new HttpError(ex, requestUri.ToString(), null, $"HTTP PATCH request failed for URI: {requestUri}");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new HttpError(ex, requestUri.ToString(), null, $"HTTP PATCH request timed out for URI: {requestUri}");
        }
        catch (TaskCanceledException ex)
        {
            return new HttpError(ex, requestUri.ToString(), null, $"HTTP PATCH request was cancelled for URI: {requestUri}");
        }
        catch (Exception ex)
        {
            return new HttpError(ex, requestUri.ToString(), null, $"Unexpected error during HTTP PATCH request to URI: {requestUri}");
        }
    }

    /// <summary>
    /// Attempts to send a DELETE request to the specified Uri, returning a Maybe result.
    /// </summary>
    /// <param name="client">The HttpClient to use for the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <returns>A Maybe containing the HttpResponseMessage or an HttpError.</returns>
    public static async Task<Maybe<HttpResponseMessage, HttpError>> TryDeleteAsync(this HttpClient client, string? requestUri)
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
            var response = await client.DeleteAsync(requestUri).ConfigureAwait(false);
            return Maybe<HttpResponseMessage, HttpError>.Some(response);
        }
        catch (HttpRequestException ex)
        {
            return new HttpError(ex, requestUri, null, $"HTTP DELETE request failed for URI: {requestUri}");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new HttpError(ex, requestUri, null, $"HTTP DELETE request timed out for URI: {requestUri}");
        }
        catch (TaskCanceledException ex)
        {
            return new HttpError(ex, requestUri, null, $"HTTP DELETE request was cancelled for URI: {requestUri}");
        }
        catch (Exception ex)
        {
            return new HttpError(ex, requestUri, null, $"Unexpected error during HTTP DELETE request to URI: {requestUri}");
        }
    }

    /// <summary>
    /// Attempts to send a DELETE request to the specified Uri, returning a Maybe result.
    /// </summary>
    /// <param name="client">The HttpClient to use for the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <returns>A Maybe containing the HttpResponseMessage or an HttpError.</returns>
    public static async Task<Maybe<HttpResponseMessage, HttpError>> TryDeleteAsync(this HttpClient client, Uri? requestUri)
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
            var response = await client.DeleteAsync(requestUri).ConfigureAwait(false);
            return Maybe<HttpResponseMessage, HttpError>.Some(response);
        }
        catch (HttpRequestException ex)
        {
            return new HttpError(ex, requestUri.ToString(), null, $"HTTP DELETE request failed for URI: {requestUri}");
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new HttpError(ex, requestUri.ToString(), null, $"HTTP DELETE request timed out for URI: {requestUri}");
        }
        catch (TaskCanceledException ex)
        {
            return new HttpError(ex, requestUri.ToString(), null, $"HTTP DELETE request was cancelled for URI: {requestUri}");
        }
        catch (Exception ex)
        {
            return new HttpError(ex, requestUri.ToString(), null, $"Unexpected error during HTTP DELETE request to URI: {requestUri}");
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

    #region JSON-integrated HTTP methods

    /// <summary>
    /// Attempts to send a GET request and deserialize the JSON response to the specified type, returning a Maybe result.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the JSON response to.</typeparam>
    /// <param name="client">The HttpClient to use for the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="options">Optional JsonSerializerOptions for deserialization.</param>
    /// <returns>A Maybe containing the deserialized object or an HttpJsonError.</returns>
    public static async Task<Maybe<T, HttpJsonError>> TryGetJsonAsync<T>(this HttpClient client, string? requestUri, JsonSerializerOptions? options = null)
    {
        var stringResult = await client.TryGetStringAsync(requestUri).ConfigureAwait(false);
        
        if (stringResult.IsError)
        {
            return new HttpJsonError(stringResult.ErrorOrThrow());
        }

        var jsonResult = JsonToolkit.TryDeserialize<T>(stringResult.ValueOrThrow(), options);
        if (jsonResult.IsError)
        {
            return new HttpJsonError(jsonResult.ErrorOrThrow());
        }

        return Maybe<T, HttpJsonError>.Some(jsonResult.ValueOrThrow());
    }

    /// <summary>
    /// Attempts to send a GET request and deserialize the JSON response to the specified type, returning a Maybe result.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the JSON response to.</typeparam>
    /// <param name="client">The HttpClient to use for the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="options">Optional JsonSerializerOptions for deserialization.</param>
    /// <returns>A Maybe containing the deserialized object or an HttpJsonError.</returns>
    public static async Task<Maybe<T, HttpJsonError>> TryGetJsonAsync<T>(this HttpClient client, Uri? requestUri, JsonSerializerOptions? options = null)
    {
        return await client.TryGetJsonAsync<T>(requestUri?.ToString(), options).ConfigureAwait(false);
    }

    /// <summary>
    /// Attempts to serialize an object to JSON and send it as a POST request, returning a Maybe result.
    /// </summary>
    /// <typeparam name="TRequest">The type of the object to serialize and send.</typeparam>
    /// <param name="client">The HttpClient to use for the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The object to serialize and send.</param>
    /// <param name="options">Optional JsonSerializerOptions for serialization.</param>
    /// <returns>A Maybe containing the HttpResponseMessage or an HttpJsonError.</returns>
    public static async Task<Maybe<HttpResponseMessage, HttpJsonError>> TryPostJsonAsync<TRequest>(this HttpClient client, string? requestUri, TRequest value, JsonSerializerOptions? options = null)
    {
        var jsonResult = JsonToolkit.TrySerialize(value, options);
        if (jsonResult.IsError)
        {
            return new HttpJsonError(jsonResult.ErrorOrThrow());
        }

        var content = new StringContent(jsonResult.ValueOrThrow(), Encoding.UTF8, "application/json");
        var httpResult = await client.TryPostAsync(requestUri, content).ConfigureAwait(false);
        
        if (httpResult.IsError)
        {
            return new HttpJsonError(httpResult.ErrorOrThrow());
        }

        return Maybe<HttpResponseMessage, HttpJsonError>.Some(httpResult.ValueOrThrow());
    }

    /// <summary>
    /// Attempts to serialize an object to JSON and send it as a POST request, then deserialize the response, returning a Maybe result.
    /// </summary>
    /// <typeparam name="TRequest">The type of the object to serialize and send.</typeparam>
    /// <typeparam name="TResponse">The type to deserialize the JSON response to.</typeparam>
    /// <param name="client">The HttpClient to use for the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The object to serialize and send.</param>
    /// <param name="options">Optional JsonSerializerOptions for serialization/deserialization.</param>
    /// <returns>A Maybe containing the deserialized response object or an HttpJsonError.</returns>
    public static async Task<Maybe<TResponse, HttpJsonError>> TryPostJsonAsync<TRequest, TResponse>(this HttpClient client, string? requestUri, TRequest value, JsonSerializerOptions? options = null)
    {
        var responseResult = await client.TryPostJsonAsync(requestUri, value, options).ConfigureAwait(false);
        
        if (responseResult.IsError)
        {
            return responseResult.ErrorOrThrow();
        }

        try
        {
            using (var response = responseResult.ValueOrThrow())
            {
                var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var jsonResult = JsonToolkit.TryDeserialize<TResponse>(jsonString, options);
                
                if (jsonResult.IsError)
                {
                    return new HttpJsonError(jsonResult.ErrorOrThrow());
                }

                return Maybe<TResponse, HttpJsonError>.Some(jsonResult.ValueOrThrow());
            }
        }
        catch (Exception ex)
        {
            return new HttpJsonError(new HttpError(ex, requestUri, null, $"Failed to read response content from URI: {requestUri}"));
        }
    }

    /// <summary>
    /// Attempts to serialize an object to JSON and send it as a PUT request, returning a Maybe result.
    /// </summary>
    /// <typeparam name="TRequest">The type of the object to serialize and send.</typeparam>
    /// <param name="client">The HttpClient to use for the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The object to serialize and send.</param>
    /// <param name="options">Optional JsonSerializerOptions for serialization.</param>
    /// <returns>A Maybe containing the HttpResponseMessage or an HttpJsonError.</returns>
    public static async Task<Maybe<HttpResponseMessage, HttpJsonError>> TryPutJsonAsync<TRequest>(this HttpClient client, string? requestUri, TRequest value, JsonSerializerOptions? options = null)
    {
        var jsonResult = JsonToolkit.TrySerialize(value, options);
        if (jsonResult.IsError)
        {
            return new HttpJsonError(jsonResult.ErrorOrThrow());
        }

        var content = new StringContent(jsonResult.ValueOrThrow(), Encoding.UTF8, "application/json");
        var httpResult = await client.TryPutAsync(requestUri, content).ConfigureAwait(false);
        
        if (httpResult.IsError)
        {
            return new HttpJsonError(httpResult.ErrorOrThrow());
        }

        return Maybe<HttpResponseMessage, HttpJsonError>.Some(httpResult.ValueOrThrow());
    }

    /// <summary>
    /// Attempts to serialize an object to JSON and send it as a PUT request, then deserialize the response, returning a Maybe result.
    /// </summary>
    /// <typeparam name="TRequest">The type of the object to serialize and send.</typeparam>
    /// <typeparam name="TResponse">The type to deserialize the JSON response to.</typeparam>
    /// <param name="client">The HttpClient to use for the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The object to serialize and send.</param>
    /// <param name="options">Optional JsonSerializerOptions for serialization/deserialization.</param>
    /// <returns>A Maybe containing the deserialized response object or an HttpJsonError.</returns>
    public static async Task<Maybe<TResponse, HttpJsonError>> TryPutJsonAsync<TRequest, TResponse>(this HttpClient client, string? requestUri, TRequest value, JsonSerializerOptions? options = null)
    {
        var responseResult = await client.TryPutJsonAsync(requestUri, value, options).ConfigureAwait(false);
        
        if (responseResult.IsError)
        {
            return responseResult.ErrorOrThrow();
        }

        try
        {
            using (var response = responseResult.ValueOrThrow())
            {
                var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var jsonResult = JsonToolkit.TryDeserialize<TResponse>(jsonString, options);
                
                if (jsonResult.IsError)
                {
                    return new HttpJsonError(jsonResult.ErrorOrThrow());
                }

                return Maybe<TResponse, HttpJsonError>.Some(jsonResult.ValueOrThrow());
            }
        }
        catch (Exception ex)
        {
            return new HttpJsonError(new HttpError(ex, requestUri, null, $"Failed to read response content from URI: {requestUri}"));
        }
    }

    /// <summary>
    /// Attempts to serialize an object to JSON and send it as a PATCH request, returning a Maybe result.
    /// </summary>
    /// <typeparam name="TRequest">The type of the object to serialize and send.</typeparam>
    /// <param name="client">The HttpClient to use for the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The object to serialize and send.</param>
    /// <param name="options">Optional JsonSerializerOptions for serialization.</param>
    /// <returns>A Maybe containing the HttpResponseMessage or an HttpJsonError.</returns>
    public static async Task<Maybe<HttpResponseMessage, HttpJsonError>> TryPatchJsonAsync<TRequest>(this HttpClient client, string? requestUri, TRequest value, JsonSerializerOptions? options = null)
    {
        var jsonResult = JsonToolkit.TrySerialize(value, options);
        if (jsonResult.IsError)
        {
            return new HttpJsonError(jsonResult.ErrorOrThrow());
        }

        var content = new StringContent(jsonResult.ValueOrThrow(), Encoding.UTF8, "application/json");
        var httpResult = await client.TryPatchAsync(requestUri, content).ConfigureAwait(false);
        
        if (httpResult.IsError)
        {
            return new HttpJsonError(httpResult.ErrorOrThrow());
        }

        return Maybe<HttpResponseMessage, HttpJsonError>.Some(httpResult.ValueOrThrow());
    }

    /// <summary>
    /// Attempts to serialize an object to JSON and send it as a PATCH request, then deserialize the response, returning a Maybe result.
    /// </summary>
    /// <typeparam name="TRequest">The type of the object to serialize and send.</typeparam>
    /// <typeparam name="TResponse">The type to deserialize the JSON response to.</typeparam>
    /// <param name="client">The HttpClient to use for the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The object to serialize and send.</param>
    /// <param name="options">Optional JsonSerializerOptions for serialization/deserialization.</param>
    /// <returns>A Maybe containing the deserialized response object or an HttpJsonError.</returns>
    public static async Task<Maybe<TResponse, HttpJsonError>> TryPatchJsonAsync<TRequest, TResponse>(this HttpClient client, string? requestUri, TRequest value, JsonSerializerOptions? options = null)
    {
        var responseResult = await client.TryPatchJsonAsync(requestUri, value, options).ConfigureAwait(false);
        
        if (responseResult.IsError)
        {
            return responseResult.ErrorOrThrow();
        }

        try
        {
            using (var response = responseResult.ValueOrThrow())
            {
                var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var jsonResult = JsonToolkit.TryDeserialize<TResponse>(jsonString, options);
                
                if (jsonResult.IsError)
                {
                    return new HttpJsonError(jsonResult.ErrorOrThrow());
                }

                return Maybe<TResponse, HttpJsonError>.Some(jsonResult.ValueOrThrow());
            }
        }
        catch (Exception ex)
        {
            return new HttpJsonError(new HttpError(ex, requestUri, null, $"Failed to read response content from URI: {requestUri}"));
        }
    }

    #endregion
}