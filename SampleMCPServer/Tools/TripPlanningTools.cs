using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelContextProtocol.Server;

namespace SampleMCPServer.Tools
{
    internal class TripPlanningTools
    {
        [McpServerTool]
        [Description("Plan a trip on a location on a given day.")]
        public string GetRandomNumber(
        [Description("Location")] string location,
        [Description("The date")] DateTime date)
        {
            return "go and have fun";
        }
    }
}
