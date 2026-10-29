using Dapper;
using Microsoft.Data.SqlClient;

/// <summary>
/// 封裝資料庫的操作
/// </summary>
public class Database
{
  const string orderSelectSql = """
      select Id = [OrderId], Date = [OrderDate]
      from[Orders]
      where[OrderId] = @orderId
    """;
  const string orderProductSelectSql = """
        select
        	[Order Details].[ProductID],
        	[Products].[ProductName],
        	[Order Details].[Quantity],
        	[Order Details].[UnitPrice]
        from [northwind].[dbo].[Order Details]
        	join [northwind].[dbo].[Products] on [Products].[ProductID] = [Order Details].[ProductID]
        where [Order Details].[OrderID]=@orderId
        """;
  const string createLogTableSql = """
        if object_id(N'dbo.OrderQueryLogs', N'U') is null
        begin
            create table dbo.OrderQueryLogs
            (
                Id int identity(1, 1) primary key,
                OrderId int not null,
                QueryTime datetime not null
            );
        end;
        """;
  const string insertLogSql = """
        insert into dbo.OrderQueryLogs (OrderId, QueryTime)
        values (@orderId, @queryTime);
        """;

  private string _connectString;

  /// <summary>
  /// 初始化：使用 connectionString 連接資料庫
  /// </summary>
  /// <param name="connectionString">資料庫連接字串</param>
  public Database(string connectionString)
  {
    _connectString = connectionString;

    // 初始化Log表格
    using (var connection = new SqlConnection(_connectString))
    {
      connection.Execute(createLogTableSql);
    }
  }

  /// <summary>
  /// 取得資料庫裡的訂單資料
  /// </summary>
  /// <param name="orderId">訂單編號</param>
  /// <returns>編號對應的訂單，null代表沒有這筆訂單</returns>
  public Order? GetOrder(int orderId)
  {
    using (var connection = new SqlConnection(_connectString))
    {
      Order? order = connection.QuerySingleOrDefault<Order>(orderSelectSql, new { orderId });
      if (order is not null)
      {
        order.productList = connection.Query<OrderProduct>(orderProductSelectSql, new { orderId }).ToList();
      }
      return order;
    }
  }

  /// <summary>
  /// 新增一筆查詢 Log 資料
  /// </summary>
  /// <param name="orderId">訂單編號</param>
  /// <param name="queryTime">查詢時間</param>
  public void InsertLog(int orderId, DateTime queryTime)
  {
    using (var connection = new SqlConnection(_connectString))
    {
      connection.Execute(insertLogSql, new { orderId, queryTime });
    }
  }
}
