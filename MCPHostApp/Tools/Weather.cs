using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;

namespace MCPHostApp.Tools
{
    internal class Weather
    {
        public static AIFunction Create()
        {
            var queryWeatherFunc = AIFunctionFactory.Create(
                new Func<string, CancellationToken, Task<string>>(async (location, ct) =>
                {
                    using var httpClient = HttpClientFactory.Create();
                    var url = $"https://localhost:7147/WeatherForecast?cityName={Uri.EscapeDataString(location)}";
                    var data = await httpClient.GetStringAsync(url, ct);
                    return data;
                }),
                new AIFunctionFactoryOptions
                {
                    Name = "query_weather",
                    Description = """
                    This tool is to get weather forecast for a given location. The result would include all things around weather
                    like temperature, coolness for several days so that you could choose the best days. 

                    ###Expected input:
                    You need to pass the location name to invoke the tool
                    Example: {
                        "location": "Da Nang"
                    }

                    ### Usage instruction for the LLM
                    Use this tool when you need to know the weather at a destination before recommending travel plan.
                    """
                }
            );

            return queryWeatherFunc;
        }
    }
}
