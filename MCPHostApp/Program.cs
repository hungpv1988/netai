using System.Net.Http;
using Azure.AI.OpenAI;
using Azure.Identity;
using MCPHostApp.Tools;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;

using ModelContextProtocol.Client;
using OpenAI;

var config = new ConfigurationBuilder()
    .AddUserSecrets("sharedconfigs") 
    .Build();
// Initialize OpenAI client with your API key
var client = new OpenAIClient(config["OPENAI_API_KEY"]);
// Build IChatClient for GPT-4o with function invocation enabled
IChatClient chatClient =
    new ChatClientBuilder(
        client.GetChatClient("gpt-4o").AsIChatClient()
    )
    .UseFunctionInvocation()
    .Build();

// Create the MCP client
// Configure it to start and connect to your MCP server.
IMcpClient mcpClient = await McpClientFactory.CreateAsync(
    new StdioClientTransport(new()
    {
        Command = "dotnet run",
        Arguments = ["--project", "C:\\Documents\\Tech\\netai\\SampleMCPServer"],
        Name = "Minimal MCP Server",
    }));

// List all available tools from the MCP server.
Console.WriteLine("Available tools:");
IList<AITool> tools = (await mcpClient.ListToolsAsync()).Cast<AITool>().ToList();


// Create an AIFunction from a delegate that calls your localhost weather service
var queryWeatherFunc = AIFunctionFactory.Create(
    new Func<string, CancellationToken, Task<string>>(async (location, ct) =>
    {
        using var httpClient = HttpClientFactory.Create();
        var url = $"https://localhost:7147/WeatherForecast?cityName={Uri.EscapeDataString(location)}";
        var data= await httpClient.GetStringAsync(url, ct);
        return data;
    }),
    new AIFunctionFactoryOptions
    {
        Name = "query_weather",
      //  Description = "Query all things around weather like temperature, coolness. Parameters: location (string))."
        // You can set MarshalResult or ConfigureParameterBinding here if needed.
    }
);

// Create an AIFunction from a delegate that calls your localhost location service
var queryLocationFunc = AIFunctionFactory.Create(
    new Func<CancellationToken, Task<string>>(async (ct) =>
    {
        using var httpClient = HttpClientFactory.Create();
        var url = $"https://localhost:7147/Location";
        var data = await httpClient.GetStringAsync(url, ct);
        return data;
    }),
    new AIFunctionFactoryOptions
    {
        Name = "query_location",
        //  Description = "Query all things around weather like temperature, coolness. Parameters: location (string))."
        // You can set MarshalResult or ConfigureParameterBinding here if needed.
    }
);

tools.Add(queryWeatherFunc);
tools.Add(queryLocationFunc);
tools.Add(SaleData.Create());
tools.Add(Alert.Create());
tools.Add(ProcessSale.Create());
Console.WriteLine($"Total tools: {tools.Count}");
foreach (AITool tool in tools)
{
    Console.WriteLine($"{tool}");
}
Console.WriteLine();
//var filePath = Path.Combine(AppContext.BaseDirectory, "guide.md");

//if (!File.Exists(filePath))
//{
//    Console.WriteLine($"guide.md not found at: {filePath}");
//    Console.WriteLine("Working directory: " + Directory.GetCurrentDirectory());
//    return;
//}

//var guide = File.ReadAllText("callservice.md");

//var guide = File.ReadAllText("guide.md");
var guide = File.ReadAllText("travelguide.md");

// Conversational loop that can utilize the tools via prompts.
List<ChatMessage> messages = [];
while (true)
{
    Console.Write("Prompt: ");
    messages.Add(new(ChatRole.System, guide));
    messages.Add(new(ChatRole.User, Console.ReadLine()));

    List<ChatResponseUpdate> updates = [];
    await foreach (ChatResponseUpdate update in chatClient
        .GetStreamingResponseAsync(messages, new() { Tools = [.. tools] }))
    {
        Console.Write(update);
        updates.Add(update);
    }
    Console.WriteLine();

    messages.AddMessages(updates);
}

