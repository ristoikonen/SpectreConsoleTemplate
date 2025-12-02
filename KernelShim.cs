using System.Threading.Tasks;

namespace SpectreConsoleTEMPL;

// Minimal shim to mimic registering a chat completion provider and requesting completions.
// This is NOT a full Semantic Kernel implementation, but provides a simple abstraction
// that the sample Program.cs can call without pulling the full Semantic Kernel package.
public class KernelShim
{
    private readonly System.Collections.Generic.Dictionary<string, OllamaClient> _providers = new();
    private readonly System.Collections.Generic.List<IChatPlugin> _plugins = new();

    public void AddOllamaChatCompletion(string name, OllamaClient client)
    {
        _providers[name] = client;
    }

    public void AddPlugin(IChatPlugin plugin)
    {
        if (plugin is null) return;
        _plugins.Add(plugin);
    }

    public async Task<string> GetChatCompletionAsync(string providerName, string model, string prompt)
    {
        if (!_providers.TryGetValue(providerName, out var client))
            throw new System.Exception($"Provider '{providerName}' not registered.");

        // Apply plugins before sending
        var workingPrompt = prompt;
        foreach (var p in _plugins)
        {
            workingPrompt = await p.BeforeSendAsync(workingPrompt);
        }

        var response = await client.GenerateAsync(model, workingPrompt);

        // Let plugins observe/modify the response after receive
        var workingResponse = response;
        foreach (var p in _plugins)
        {
            workingResponse = await p.AfterReceiveAsync(workingResponse);
        }

        return workingResponse;
    }
}
