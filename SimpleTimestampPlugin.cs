using System.Threading.Tasks;

namespace SpectreConsoleTEMPL;

// Simple plugin that prefixes the prompt with a timestamp and appends a note to the response.
public class SimpleTimestampPlugin : IChatPlugin
{
    public Task<string> BeforeSendAsync(string prompt)
    {
        var ts = System.DateTime.UtcNow.ToString("o");
        var newPrompt = $"[timestamp:{ts}] {prompt}";
        return Task.FromResult(newPrompt);
    }

    public Task<string> AfterReceiveAsync(string response)
    {
        var note = "\n\n-- Response processed by SimpleTimestampPlugin";
        return Task.FromResult(response + note);
    }
}
