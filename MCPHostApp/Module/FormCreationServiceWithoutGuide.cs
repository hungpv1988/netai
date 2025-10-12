using Microsoft.Extensions.AI;


namespace MCPHostApp.Module
{
    internal class FormCreationServiceWithoutGuide
    {
        public static async Task Start(IChatClient chatClient, IList<AITool> tools)
        {
            //var guide = "You are an assistant that manages forms. You would complete your task by choosing the right tool" +
            //    "to create a form after an image is uploaded. Do exactly like the tool requires for data input and send good data to the tool. If things go smoothly, use polite words to inform users and show the final result returned in json format";

            // push file to chat
            var imageBytes = await File.ReadAllBytesAsync("C:\\Documents\\RegistrationWithOption.png");

            // Conversational loop that can utilize the tools via prompts.
            List<ChatMessage> messages = [];
            bool isUpload = false;

            while (true)
            {
                // we have no guide for LLM, so LLM would figure out what to do based on the tool definition
                // the reason why we need a detailed tool defintion in this case.
                // Note that both with and without guide, we use the same tool, so the detailed tool definition is not needed for the case with guide FormCreationServiceWithGuide
                Console.Write("Prompt: ");
                messages.Add(new(ChatRole.User, Console.ReadLine()));
                if (!isUpload)
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
        }
    }
}
