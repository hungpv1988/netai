using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;

namespace MCPHostApp.Tools
{
    internal class Location
    {
        public static AIFunction Create()
        {
            var queryLocationFunc = AIFunctionFactory.Create(
                 new Func<CancellationToken, Task<string>>(async (ct) =>
                 {
                     using var httpClient = HttpClientFactory.Create();
                     var url = $"https://localhost:7147/Location";
                     var data = await httpClient.GetStringAsync(url, ct);
                     return data;
                 }),
                 new AIFunctionFactoryOptions
                 {
                     Name = "query_location",
                     Description = """
                       Get all locations that have travel attractions. The result would include location name and description about it so that LLM could filter and choose the best locations according to user preferences.
                     """
                 }
            );

            return queryLocationFunc;
        }
    }
}
