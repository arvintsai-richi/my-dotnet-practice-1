using System.Text.Json.Serialization;

/// <summary>
/// 資料庫的訂單資訊
/// </summary>
/// <remarks>
/// 初始化訂單資訊
/// </remarks>
/// <param name="id">訂單編號</param>
/// <param name="date">訂購日期</param>
/// <param name="productList">訂購商品清單</param>
public class Order(int id, DateTime date, List<OrderProduct> productList)
{
  /// <summary>
  /// 訂單編號
  /// </summary>
  [JsonPropertyName("訂單編號")]
  public int Id { get; set; } = id;
  /// <summary>
  /// 訂購日期
  /// </summary>
  [JsonPropertyName("訂購日期")]
  public string Date { get; set; } = date.ToString("yyyy/M/d");
  /// <summary>
  /// 訂購金額
  /// </summary>
  [JsonPropertyName("訂購金額")]
  public decimal Total => productList.Sum(product => product.Quantity * product.UnitPrice);
  /// <summary>
  /// 訂購商品清單
  /// </summary>
  [JsonPropertyName("訂購商品清單")]
  public List<OrderProduct> ProductList { get; set; } = productList ?? [];
}
