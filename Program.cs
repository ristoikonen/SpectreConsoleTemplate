using Spectre.Console;
using Spectre.Console.Cli;
// using Microsoft.Extensions.Configuration;



namespace SpectreConsoleTEMPL;

public static class Program
{
    static async Task Main(string[] args)
    {
        // ConfigurationManager configurationManager = new ConfigurationManager();
        //var configvalue1 = configurationManager.Sources; 

        string title = "title";

        SpectreConsoleOutput.DisplayTitleH3(title);

        // user choice scenarios
        var scenarios = SpectreConsoleOutput.SelectScenarios();
        var scenario = scenarios[0];

        // present
        switch (scenario)
        {
            case "PDF AI Summariser":

                break;
            case "Get Response":

                break;
            case "Generate image":

                break;

            case "AddOllamaChatCompletion":

                // Simple Semantic Kernel-style registration and chat completion using Ollama
                var baseUrl = AnsiConsole.Ask<string>("Ollama base URL (press enter for http://localhost:11434):");
                if (string.IsNullOrWhiteSpace(baseUrl)) baseUrl = "http://localhost:11434";

                var modelName = AnsiConsole.Ask<string>("Model name to use (eg. llama3.2):");
                if (string.IsNullOrWhiteSpace(modelName)) modelName = "llama3.2";

                var promptText = AnsiConsole.Ask<string>("Prompt to send to the model:");

                var kernel = new KernelShim();
                var ollamaClient = new OllamaClient(baseUrl);
                kernel.AddOllamaChatCompletion("ollama", ollamaClient);
                // register a simple plugin
                kernel.AddPlugin(new SimpleTimestampPlugin());

                try
                {
                    var completion = await kernel.GetChatCompletionAsync("ollama", modelName, promptText);
                    AnsiConsole.MarkupLine("[bold blue]Completion:[/]");
                    AnsiConsole.WriteLine(completion);
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }

                break;

            case "IChatClient":

                // Run a simple Ollama-based chat loop
                await OllamaChat.RunAsync();

                break;
        }
    }
}
