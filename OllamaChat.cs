using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Spectre.Console;

namespace SpectreConsoleTEMPL;

public static class OllamaChat
{
    // Simple chat loop using a local Ollama server at http://localhost:11434
    public static async Task RunAsync()
    {
        AnsiConsole.MarkupLine("[bold]Starting Ollama chat (local server http://localhost:11434)[/]");

        var client = new OllamaClient("http://localhost:11434");

        var history = new List<(string role, string content)>();

        while (true)
        {
            var prompt = AnsiConsole.Ask<string>($"exit to quit"); // [green]You[/]: (type 'exit' to quit)[/]
            if (string.Equals(prompt, "exit", StringComparison.OrdinalIgnoreCase))
                break;

            history.Add(("user", prompt));

            AnsiConsole.Status()
                .Start("Waiting for model...", ctx => { ctx.Spinner(Spinner.Known.Dots); });

            try
            {
                var response = await client.GenerateAsync("llama3.2", prompt);
                if (!string.IsNullOrEmpty(response))
                {
                    history.Add(("assistant", response));

                    AnsiConsole.MarkupLine("[bold blue]Assistant:[/]");
                    AnsiConsole.WriteLine(response);
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]No response from model[/]");
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
            }
        }

        AnsiConsole.MarkupLine("[grey]Chat ended[/]");
    }
}

public class OllamaClient
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;

    public OllamaClient(string baseUrl)
    {
        _baseUrl = baseUrl.TrimEnd('/');
        _http = new HttpClient();
        _http.BaseAddress = new Uri(_baseUrl);
        _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<string> GenerateAsync(string model, string prompt)
    {
        // Ollama's /api/generate expects a JSON body like { "model": "<model>", "prompt": "..." }
        var payload = new
        {
            model = model,
            prompt = prompt,
            // optional: max_tokens, temperature, etc.
        };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var resp = await _http.PostAsync("/api/generate", content);
        if (!resp.IsSuccessStatusCode)
        {
            var txt = await resp.Content.ReadAsStringAsync();
            throw new Exception($"Ollama returned {(int)resp.StatusCode}: {txt}");
        }

        var body = await resp.Content.ReadAsStringAsync();

        // The response from Ollama can be a streaming multipart or JSON; try to parse simple JSON with "response" or just return body.
        try
        {
            using var doc = JsonDocument.Parse(body);
            if (doc.RootElement.TryGetProperty("response", out var respProp))
            {
                return respProp.GetString() ?? body;
            }
            // some versions return { "choices": [ { "text": "..." } ] }
            if (doc.RootElement.TryGetProperty("choices", out var choices) && choices.ValueKind == JsonValueKind.Array && choices.GetArrayLength() > 0)
            {
                var first = choices[0];
                if (first.TryGetProperty("text", out var text))
                    return text.GetString() ?? body;
            }

            return body;
        }
        catch
        {
            // not JSON
            return body;
        }
    }
}
