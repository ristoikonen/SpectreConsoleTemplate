using System.Threading.Tasks;

namespace SpectreConsoleTEMPL;

public interface IChatPlugin
{
    // Called before the prompt is sent to the model. Return modified prompt.
    Task<string> BeforeSendAsync(string prompt);

    // Called after a response is received. Return modified response.
    Task<string> AfterReceiveAsync(string response);
}
