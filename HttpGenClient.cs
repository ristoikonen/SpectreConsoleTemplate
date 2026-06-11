using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpectreConsoleTEMPL;


internal class HttpGenClient<TResponse>(HttpClient httpClient)
{

    HttpClient httpClient = httpClient;

    /// <summary>
    /// HTTP GET data of model type TResponse
    /// </summary>
    /// <param name="requestUri">GetAsync's requestUri</param>
    /// <param name="hostHeader">Host header</param>
    /// <param name="token">Authorization header token, Note: string 'Bearer ' added in the front.</param>
    /// <returns>Data of model type TResponse or default</returns>
    public async Task<TResponse?> GetAsync(
        string requestUri,
        string hostHeader,
        Dictionary<string, string>? queryParams = null,
        string token = "")
    {
        try
        {
            var targetUri = requestUri;
            if (queryParams is { Count: > 0 })
            {
                var queryString = new FormUrlEncodedContent(queryParams).ReadAsStringAsync().Result;
                targetUri = $"{requestUri}?{queryString}";
            }

            using var request = new HttpRequestMessage(HttpMethod.Get, targetUri);

            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            request.Headers.Host = hostHeader;
            request.Headers.Connection.Add("keep-alive");

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            using HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<TResponse>(contentStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return default;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"HTTP Request failed: {ex.Message}");
            return default;
        }
    }
}
