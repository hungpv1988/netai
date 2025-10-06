
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;




public class SentimentChat
{
   
    public SentimentChat()
    {
       
    }
    public async Task GetSentimentAsync()
    {
        var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        string model = config["ModelName"];
        string key = config["OpenAIKey"];
        var categories = new[] { "Positive", "Negative", "Neutral", "Mixed" };
        string categoryList = string.Join(", ", categories);
        IChatClient chatClient = new OpenAIClient(key).GetChatClient(model).AsIChatClient();
        string review = "fuck with the product!";
        var messages = new List<ChatMessage>
        {
            new ChatMessage(ChatRole.System,$"You are a classifier. Always respond with exactly one of these categories: {categoryList}."),
            new ChatMessage(ChatRole.User, review)
        };
       
        var response = await chatClient.GetResponseAsync($"What's the sentiment of this review? {review}");
        
        foreach (var message in response.Messages)
        {
            Console.WriteLine($"{message.Role}: {message.Text}");
        };
    }

    public async Task GetSentimentFromChatAsync()
    {
        var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        string model = config["ModelName"];
        string key = config["OpenAIKey"];
        var categories = new[] { "Positive", "Negative", "Neutral", "Mixed" };
        string categoryList = string.Join(", ", categories);
        IChatClient chatClient = new OpenAIClient(key).GetChatClient(model).AsIChatClient();
        var chatHistory = new List<ChatMessage>
        {
              new ChatMessage(ChatRole.System,$"You are a classifier. Always respond with exactly one of these categories: {categoryList}.")
        };

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