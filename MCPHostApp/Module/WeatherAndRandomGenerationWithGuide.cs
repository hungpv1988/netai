using Microsoft.Extensions.AI;


namespace MCPHostApp.Module
{
    internal class WeatherAndRandomGenerationWithGuide
    {
        public static async Task Start(IChatClient chatClient, IList<AITool> tools)
        {
            var guide = File.ReadAllText("callservice.md");

            // Conversational loop that can utilize the tools via prompts.
            List<ChatMessage> messages = [];
            bool isUpload = false;

            while (true)
            {

                Console.Write("Prompt: ");
                messages.Add(new(ChatRole.System, guide)); //could ignore guide if tools define clearly. LLM would handle the rest.
                messages.Add(new(ChatRole.User, Console.ReadLine()));

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
        }
    }
}
