using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using System.Text.Json;
using MCPHostApp.Tools.Records; // Add this if not present

namespace MCPHostApp.Tools
{


    internal class SaleData
    {
        public static AIFunction Create()
        {
            //
            // 🔹 Tool 1: Lấy dữ liệu và gợi ý hướng xử lý
            //
            var getSalesTool = AIFunctionFactory.Create(
                (string month) =>
                {
                    Console.WriteLine($"📦 [get_sales_data] Lấy dữ liệu tháng {month}");
                    var data = new SalesData(month, new[] { 800, 750, 600, 550, 500 }); // giả sử doanh số giảm

                    var nextPrompt = $"""
                    Đây là dữ liệu bán hàng tháng {month}: {JsonSerializer.Serialize(data)}.
                    Hãy phân tích dữ liệu này để tìm xu hướng, trung bình, và điểm đỉnh.
                    Nếu kết quả cho thấy doanh số đang giảm, hãy chọn một công cụ phù hợp để gửi cảnh báo tới đội kinh doanh.
                """;

                    return new ProcessPlan(JsonSerializer.Serialize(data), nextPrompt);
                },
                name: "get_sales_data",
                description: "Lấy dữ liệu bán hàng thô của một tháng và đề xuất cách xử lý tiếp theo."
            );

            return getSalesTool;
        }
    }
}
