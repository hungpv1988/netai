using Microsoft.Extensions.AI;


namespace MCPHostApp.Module
{
    internal class FormCreationServiceWithGuide
    {
        /// <summary>
        /// With this guide, LLM would know how to use the tool to help user create a form from an image.
        /// </summary>
        /// <param name="chatClient"></param>
        /// <param name="tools"></param>
        /// <returns></returns>
        public static async Task Start(IChatClient chatClient, IList<AITool> tools)
        {
            // note that we use the same tool for both with guide and without guide, so the tool needs to have a good definition
            // to work without guide (FormCreationServiceWithoutGuide). But of this case, the tool basically do not need much info like that as we guide LLM to call the service 
            // and comply with the json format required by the tool.
            // See the form.md for the guide content.
            var guide = File.ReadAllText("form.md");

            // push file to chat
            var imageBytes = await File.ReadAllBytesAsync("C:\\Documents\\RegistrationWithOption.png");

            // Conversational loop that can utilize the tools via prompts.
            List<ChatMessage> messages = [];
            bool isUpload = false;

            while (true)
            {

                Console.Write("Prompt: ");
                messages.Add(new(ChatRole.System, guide)); //could ignore guide if tools define clearly. LLM would handle the rest.
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
