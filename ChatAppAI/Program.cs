using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.AI;
using OpenAI;


Console.WriteLine("Hello, World!");

//HikeChat hikeChat = new HikeChat("Hello", "User");
//await hikeChat.InitializeChat();

SentimentChat sentimentChat = new SentimentChat();
await sentimentChat.GetSentimentFromChatAsync();
