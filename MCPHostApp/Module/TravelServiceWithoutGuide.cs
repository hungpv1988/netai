using Microsoft.Extensions.AI;


namespace MCPHostApp.Module
{
    internal class TravelServiceWithoutGuide
    {
        public static async Task Start(IChatClient chatClient, IList<AITool> tools)
        {
            // Conversational loop that can utilize the tools via prompts.
            List<ChatMessage> messages = [];
            bool isUpload = false;

            while (true)
            {
                // We have a detailed tool definition, so LLM would figure out what to do based on the tool definition
                // without a guide, the detailed tool definition is needed and LLM would handle the rest.
                // But with a guide, it could coordiate tools better and produce a better results. And in this case, we 
                // do not need a very detailed tool definition as the guide would help LLM to understand how to use the tools.
                // Note that TravelServiceWithoutGuide and TravelServiceWithGuide use the same tools, so the tool definition needs to be good enough to work without a guide.
                Console.Write("Prompt: ");
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
