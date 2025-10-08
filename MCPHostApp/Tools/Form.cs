using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;

namespace MCPHostApp.Tools
{
    internal class Form
    {
        public static AIFunction Create()
        {
            var createForm = AIFunctionFactory.Create(
                new Func<string, CancellationToken, Task<string>>(async (payload, ct) =>
                {
                    using var httpClient = HttpClientFactory.Create();
                    var url = $"https://localhost:7147/Form";
                    var content = new StringContent(payload, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(url, content);

                    var result= response.Content.ToString();
                    return result;
                }),
                new AIFunctionFactoryOptions
                {
                    Name = "create_form",
                    //  Description = "Query all things around weather like temperature, coolness. Parameters: location (string))."
                    // You can set MarshalResult or ConfigureParameterBinding here if needed.
                }
            );

            return createForm;
        }
    }
}
