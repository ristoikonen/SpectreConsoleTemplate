using System.Net.Http.Headers;
using System.Text;


namespace SpectreConsoleTEMPL;

internal class HttpGenericClient<T>(HttpClient _httpClient)
{

    /// <summary>
    /// HTTP GET data of model type T
    /// </summary>
    /// <param name="requestUri">GetAsync's requestUri</param>
    /// <param name="hostHeader">Host header</param>
    /// <param name="contentsParams">Parameters of type application/json</param>
    /// <param name="token">Authorization header token, Note: string 'Bearer ' added in the front.</param>
    /// <returns>Data of model type T or default(T)</returns>
    public async Task<T?> GetAsync(string requestUri, string hostHeader, Dictionary<string, string>? contentsParams = null, string token = "")
    {
        try
        {
            using (var client = _httpClient)
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                client.DefaultRequestHeaders.Add("Host", hostHeader);
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                if (token.Length > 0)
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                if (contentsParams is not null)
                {
                    var content = new FormUrlEncodedContent(contentsParams);
                    string paramAsJSON = System.Text.Json.JsonSerializer.Serialize(content);
                    var contents = new StringContent(paramAsJSON, Encoding.UTF8, "application/json");
                }

                HttpResponseMessage response = await client.GetAsync(requestUri);
                if (response.IsSuccessStatusCode && response.Content is object)
                {
                    var contentStream = await response.Content.ReadAsStreamAsync();
                    if(contentStream is not null)
                        return await System.Text.Json.JsonSerializer.DeserializeAsync<T>(contentStream);
                    else
                        return default(T);
                }
                else
                {
                    return default(T);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return default(T);
        }
    }
}

