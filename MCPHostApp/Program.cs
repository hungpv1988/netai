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

//var guide = File.ReadAllText("callservice.md");

//var guide = File.ReadAllText("guide.md");
//var guide = File.ReadAllText("travelguide.md");
//var guide = File.ReadAllText("form.md");  load file md to create a from
// sample of how to use instruction in-line, not use md
//var guide = "You are an assistant that manages forms. You would complete your task by choosing the right tool" +
//    "to create a form after an image is uploaded. Do exactly like the tool requires for data input and send good data to the tool. If things go smoothly, use polite words to inform users and show the final result returned in json format";


// push file to chat
var imageBytes = await File.ReadAllBytesAsync("C:\\Documents\\RegistrationWithOption.png");

// Conversational loop that can utilize the tools via prompts.
List<ChatMessage> messages = [];
bool isUpload = false;

while (true)
{

    Console.Write("Prompt: ");
    // messages.Add(new(ChatRole.System, guide)); could ignore guide if tools define clearly. LLM would handle the rest.
    messages.Add(new(ChatRole.User, Console.ReadLine()));
    if (isUpload)
    {
        Console.WriteLine("An image is uploaded for LLM after this line is shown, then you could tell LLM to do things for you ");
        //  messages.Add(new(ChatRole.User, "I would upload an image about a form, please, extract the image and let me know the form structure. I 'd like to know how many rows the Form has, and how many columns of each row. For each column, please, extract the information about html element (e.g TextBox, TextArea, RadioButton, Button) used to display a field and field name. Please, return the result in Json Format"));
        var chatMessage = new ChatMessage();
        chatMessage.Contents.Add(new DataContent(imageBytes, "image/png"));
        // just sample, don't need the line below
        //chatMessage.Contents.Add(new TextContent("help to create a form from the uploaded image"));

        chatMessage.Role = ChatRole.User;
        messages.Add(chatMessage);

        isUpload = true;
    }

    List<ChatResponseUpdate> updates = [];
    await foreach (ChatResponseUpdate update in chatClient
        .GetStreamingResponseAsync(messages, new() { Tools = [.. tools] }))
    {
        Console.Write(update);
        updates.Add(update);
    }

    messages.AddMessages(updates);
    Console.WriteLine();
}

//await TravelServiceWithoutGuide.Start(chatClient, tools);
//await TravelServiceWithGuide.Start(chatClient, tools);
//await WeatherAndRandomGenerationWithGuide.Start(chatClient, tools);
//await FormCreationServiceWithGuide.Start(chatClient, tools);
//await FormCreationServiceWithoutGuide.Start(chatClient, tools);