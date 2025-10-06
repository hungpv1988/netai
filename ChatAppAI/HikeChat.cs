using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.AI;
using OpenAI;

public class HikeChat
{
    public string Message { get; set; }
    public string Sender { get; set; }
    public DateTime Timestamp { get; set; }

    public HikeChat(string message, string sender)
    {
        Message = message;
        Sender = sender;
        Timestamp = DateTime.Now;
    }

    public override string ToString()
    {
        return $"[{Timestamp}] {Sender}: {Message}";
    }

    public async Task InitializeChat()
    {
        var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        string model = config["ModelName"];
        string key = config["OpenAIKey"];

        IChatClient chatClient = new OpenAIClient(key).GetChatClient(model).AsIChatClient();
        // Start the conversation with context for the AI model
        List<ChatMessage> chatHistory =
            [
                new ChatMessage(ChatRole.System, """
                    You are a friendly hiking enthusiast who helps people discover fun hikes in their area.
                    You introduce yourself when first saying hello.
                    When helping people out, you always ask them for this information
                    to inform the hiking recommendation you provide:

                    1. The location where they would like to hike
                    2. What hiking intensity they are looking for

                    You will then provide three suggestions for nearby hikes that vary in length
                    after you get that information. You will also share an interesting fact about
                    the local nature on the hikes when making a recommendation. At the end of your
                    response, ask if there is anything else you can help with.
                """)
            ];

        while (true)
        {
            Console.Write("Your prompt: ");
            string userInput = Console.ReadLine() ?? "";
            chatHistory.Add(new ChatMessage(ChatRole.User, userInput));
            // Stream the AI response and add to chat history
            Console.WriteLine("AI Response:");
            string response = "";
            await foreach (ChatResponseUpdate item in
                chatClient.GetStreamingResponseAsync(chatHistory))
            {
                Console.Write(item.Text);
                response += item.Text;
            }
            chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));
            Console.WriteLine();
        };
    }
}