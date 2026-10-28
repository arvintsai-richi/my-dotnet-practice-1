// Reference: https://learn.microsoft.com/zh-tw/sql/connect/ado-net/get-started-sqlclient-driver?view=sql-server-ver17&tabs=azure-sql%2Cdotnet

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

try
{
  // 從 appsettings.json 取得連線字串
  var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
  string? connectionString = config["ConnectionString"];

  if (string.IsNullOrWhiteSpace(connectionString))
  {
    Console.Error.WriteLine("Please set ConnectionString correctly.");
    return 1;
  }

  // 初始化 db
  Database db = new(connectionString);

  while (true)
  {
    Console.WriteLine("請輸入訂單編號：");
    string? input = Console.ReadLine()?.Trim();
    if (input is null || input == "end")
    {
      break;
    }
    else if (Int32.TryParse(input, out int orderId))
    {
      // 取得資料
      Order? order = db.GetOrder(orderId);
      if (order is not null)
      {
        // 寫log
        db.InsertLog(orderId, DateTime.UtcNow);

        // 讓 JsonSerializer 正常顯示中文的設定
        var jsonSerializerOption = new JsonSerializerOptions
        {
          Encoder = JavaScriptEncoder.Create(
            UnicodeRanges.BasicLatin,
            UnicodeRanges.CjkUnifiedIdeographs
          )
        };
        Console.WriteLine($"訂單明細如下：{JsonSerializer.Serialize(order, jsonSerializerOption)}");
      }
      else
      {
        Console.WriteLine("查無訂單紀錄");
      }
    }
    else
    {
      Console.WriteLine("訂單號碼只限制輸入整數");
    }
    Console.WriteLine("輸入任意鍵查詢下一筆，或輸入「end」結束程式...");

    string? extrakey = Console.ReadLine()?.Trim();
    if (extrakey is null || extrakey == "end")
    {
      break;
    }
  }
  return 0;
}
catch (FileNotFoundException)
{
  Console.Error.WriteLine("The file appsettings.json doesn't exist.");
  return 2;
}
catch (OperationCanceledException)
{
  Console.Error.WriteLine("The operation was canceled.");
  return 2;
}
catch (SqlException ex)
{
  Console.Error.WriteLine($"SQL error {ex.Number}, connection {ex.ClientConnectionId}: " + ex.Message);
  if (ex.InnerException is not null)
  {
    Console.Error.WriteLine(ex.InnerException.Message);
  }
  return 3;
}