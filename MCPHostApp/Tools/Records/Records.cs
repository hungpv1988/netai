using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCPHostApp.Tools.Records
{
    record ProcessPlan(string Data, string NextPrompt);
    record SalesData(string Month, int[] Data);
    record ProcessedResult(string summary, double average, int peak);
}
