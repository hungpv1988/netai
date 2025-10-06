//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ModelContextProtocol.Server;

//namespace SampleMCPServer.Tools
//{
//    [Description("This tool is to get weather info for a city")]
//    internal class GetCityWeather
//    {
//        [McpServerTool]
//        public string GetWeather([Description("name of the city to return the weather for")]string cityName)
//        {
//            // Read the environment variable during tool execution.
//            // Alternatively, this could be read during startup and passed via IOptions dependency injection
//           string weather = string.Empty;
//           if (string.Equals(cityName, "Hanoi", StringComparison.OrdinalIgnoreCase))
//            {
//                return "Hot";
//            }
//            else if (string.Equals(cityName, "Helsinki", StringComparison.OrdinalIgnoreCase))
//            {
//                return "Cold";
//            }
//            else if (string.Equals(cityName, "London", StringComparison.OrdinalIgnoreCase))
//            {
//                return "Cool";
//            }

//            // Read the environment variable during tool execution.
//            // Alternatively, this could be read during startup and passed via IOptions dependency injection
//             weather = Environment.GetEnvironmentVariable("WEATHER_CHOICES");
//            if (string.IsNullOrWhiteSpace(weather))
//            {
//                weather = "balmy,rainy,stormy";
//            }

//            var weatherChoices = weather.Split(",");
//            var selectedWeatherIndex = Random.Shared.Next(0, weatherChoices.Length);

//            return $"The weather in {cityName} is {weatherChoices[selectedWeatherIndex]}.";
//        }
//    }
//}
