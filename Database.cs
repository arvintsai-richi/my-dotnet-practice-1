using Microsoft.Data.SqlClient;
using System.Data;

/// <summary>
/// 封裝資料庫的操作
/// </summary>
public class Database
{
  const string orderDateSelectSql = """
      select [OrderDate]
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
    using var connection = new SqlConnection(connectionString);
    connection.Open();

    using var insertLogQuery = new SqlCommand(createLogTableSql, connection);
    insertLogQuery.ExecuteNonQuery();
  }

  /// <summary>
  /// 取得資料庫裡的訂單資料
  /// </summary>
  /// <param name="orderId">訂單編號</param>
  /// <returns>編號對應的訂單，null代表沒有這筆訂單</returns>
  public Order? GetOrder(int orderId)
  {
    using var connection = new SqlConnection(_connectString);
    connection.Open();

    using var orderProductQuery = new SqlCommand(orderProductSelectSql, connection);
    using var orderQuery = new SqlCommand(orderDateSelectSql, connection);
    orderProductQuery.Parameters.Add("@orderId", SqlDbType.Int).Value = orderId;
    orderQuery.Parameters.Add("@orderId", SqlDbType.Int).Value = orderId;

    // 取得訂單的商品列表
    using SqlDataReader orderProductReader = orderProductQuery.ExecuteReader();
    List<OrderProduct> products = new List<OrderProduct>();
    while (orderProductReader.Read())
    {
      products.Add(new(orderProductReader.GetInt32(0), orderProductReader.GetString(1), orderProductReader.GetInt16(2), orderProductReader.GetDecimal(3)));
    }
    orderProductReader.Close();

    // 取得訂單日期
    DateTime? orderTime = (DateTime?)orderQuery.ExecuteScalar();
    if (orderTime is not null)
    {
      Order order = new(orderId, (DateTime)orderTime, products);
      return order;
    }
    return null;
  }

  /// <summary>
  /// 新增一筆查詢 Log 資料
  /// </summary>
  /// <param name="orderId">訂單編號</param>
  /// <param name="queryTime">查詢時間</param>
  public void InsertLog(int orderId, DateTime queryTime)
  {
    using var connection = new SqlConnection(_connectString);
    connection.Open();

    using var insertLogQuery = new SqlCommand(insertLogSql, connection);
    insertLogQuery.Parameters.Add("@orderId", SqlDbType.Int).Value = orderId;
    insertLogQuery.Parameters.Add("@queryTime", SqlDbType.DateTime).Value = queryTime;
    insertLogQuery.ExecuteNonQuery();
  }
}
