using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCPHostApp.Tools.Records;
using Microsoft.Extensions.AI;
using System.Text.Json;
namespace MCPHostApp.Tools
{
    internal class ProcessSale
    {
        public static AIFunction Create()
        {
            //
            // 🔹 Tool 2: Xử lý dữ liệu và cảnh báo
            //
            var processSalesTool = AIFunctionFactory.Create(
               (string jsonData) =>
               {
                   var data = JsonSerializer.Deserialize<SalesData>(jsonData)!;
                   var avg = 0.0;
                   var max = 0;
                   foreach (var d in data.Data)
                   {
                       avg += d;
                       if (d > max) max = d;
                   }
                   avg /= data.Data.Length;

                   string summary = avg < 700
                       ? "Doanh số đang giảm đáng kể, cần chú ý!"
                       : "Doanh số ổn định.";

                   Console.WriteLine($"📊 [process_sales_result] {summary}");
                   return new ProcessedResult(summary, avg, max);
               },
               name: "process_sales_result",
               description: "Phân tích dữ liệu bán hàng để tạo báo cáo gồm summary, average và peak."
           );

            return processSalesTool;
        }
    }
}
