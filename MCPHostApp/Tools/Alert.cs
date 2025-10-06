using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;

namespace MCPHostApp.Tools
{
    internal class Alert
    {
        public static AIFunction Create()
        {
            //
            // 🔹 Tool 3: Gửi cảnh báo
            //
            var sendAlertTool = AIFunctionFactory.Create(
               (string message) =>
               {
                   Console.WriteLine($"🚨 [alert_team] Đã gửi cảnh báo: {message}");
                   return $"Alert sent: {message}";
               },
               name: "send_alert",
               description: "Gửi cảnh báo tới đội kinh doanh nếu có vấn đề với doanh số."
           );
            return sendAlertTool;
        }
    }
}
