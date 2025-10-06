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
                    //  Description = "Query all things around weather like temperature, coolness. Parameters: location (string))."
                    // You can set MarshalResult or ConfigureParameterBinding here if needed.
                }
            );

            return queryWeatherFunc;
        }
    }
}
