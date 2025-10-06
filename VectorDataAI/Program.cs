using VectorDataAI.MinimalAIAssistant;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
// VectorService vectorService = new VectorService();
// await vectorService.SearchServiceByPrompt("suggest me a service to search for data");

//FunctionService functionService = new FunctionService();
//await functionService.ExecuteAsync();

MyAIAssistant myAIAssistant = new MyAIAssistant();
await myAIAssistant.Execute();