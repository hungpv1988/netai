using MCPHostApp.Module;
using MCPHostApp.Tools;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;

using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
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

tools.Add(Weather.Create());
tools.Add(Location.Create());
tools.Add(SaleData.Create());
tools.Add(Alert.Create());
tools.Add(ProcessSale.Create());
//tools.Add(Form.Create());
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



//await TravelServiceWithoutGuide.Start(chatClient, tools);
//await TravelServiceWithGuide.Start(chatClient, tools);
//await WeatherAndRandomGenerationWithGuide.Start(chatClient, tools);
//await FormCreationServiceWithGuide.Start(chatClient, tools);
await FormCreationServiceWithoutGuide.Start(chatClient, tools);